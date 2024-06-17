using System.Collections.Generic;

using Mirror;

using UnityEngine;
using UnityEngine.Assertions;

namespace Kdevaulo.CaptureTheFlag.CaptureFlagBehaviour
{
    public class FlagsController : IUpdatable, IFlagSpawner, IFlagInvaderObserver, IReinitializable
    {
        private readonly IMiniGameHandler _miniGameHandler;
        private readonly FlagSettings _settings;
        private readonly FlagSpawner _spawner;

        private bool _canHandleFlags;

        private Dictionary<FlagView, FlagModel> _flags;
        private Dictionary<IFlagInvader, int> _invadersCaptures;
        private Dictionary<IFlagInvader, float> _blockedInvaders;

        private List<IFlagInvader> _invaders;

        private int _maxFlags;

        private float _miniGameChance;
        private float _squaredRadius;

        public FlagsController(FlagSettings settings, FlagSpawner spawner, IMiniGameHandler miniGameHandler)
        {
            _settings = settings;
            _spawner = spawner;
            _miniGameHandler = miniGameHandler;

            _maxFlags = _settings.FlagsCount;
            _miniGameChance = _settings.MiniGameChance;
            _squaredRadius = _settings.FlagRadiusInUnits * _settings.FlagRadiusInUnits;

            _miniGameHandler.HandleMiniGameLost += HandleMiniGameLost;
        }

        void IFlagInvaderObserver.AddInvaders(params IFlagInvader[] invaders)
        {
            _invaders ??= new List<IFlagInvader>();
            _invadersCaptures ??= new Dictionary<IFlagInvader, int>();
            _blockedInvaders ??= new Dictionary<IFlagInvader, float>();

            _invaders.AddRange(invaders);

            foreach (var invader in invaders)
            {
                _invadersCaptures.Add(invader, 0);
            }
        }

        void IFlagSpawner.Clear(NetworkIdentity netIdentity)
        {
            _spawner.Clear(netIdentity);
        }

        void IFlagSpawner.Spawn(Color color, NetworkIdentity netIdentity)
        {
            var flagViews = _spawner.Spawn(color, netIdentity);
            SetupFlags(flagViews);
        }

        void IReinitializable.Reinitialize()
        {
            _canHandleFlags = false;

            _flags = null;
            _invaders = null;
            _invadersCaptures = null;
        }

        [Server]
        void IUpdatable.Update()
        {
            if (_canHandleFlags)
            {
                var flagsToRemove = TryCaptureFlags();

                if (flagsToRemove != null)
                {
                    foreach (var flag in flagsToRemove)
                    {
                        _flags.Remove(flag);
                        _spawner.DestroyFlag(flag);
                    }
                }
            }
        }

        private bool HandleFlagCaptured(IFlagInvader invader)
        {
            return ++_invadersCaptures[invader] == _maxFlags;
        }

        private void HandleMiniGameLost(IFlagInvader invader)
        {
            Assert.IsFalse(_blockedInvaders.ContainsKey(invader));

            _blockedInvaders.Add(invader, _settings.OnLoseDelay);
            Debug.Log("Added to block");
        }

        private bool IsInvaderBlocked(IFlagInvader invader)
        {
            if (_blockedInvaders.TryGetValue(invader, out float blockTimeLeft))
            {
                blockTimeLeft -= Time.deltaTime;

                if (blockTimeLeft <= 0)
                {
                    _blockedInvaders.Remove(invader);

                    Debug.Log("Unblocked");

                    return false;
                }

                _blockedInvaders[invader] = blockTimeLeft;

                return true;
            }

            return false;
        }

        private void SetupFlags(FlagView[] flags)
        {
            _flags ??= new Dictionary<FlagView, FlagModel>();

            foreach (var flagView in flags)
            {
                var position = _settings.GetPosition();
                flagView.SetPosition(position);

                var model = new FlagModel(position, _settings.SecondsToCapture)
                {
                    CanStartMiniGame = true
                };

                _flags.Add(flagView, model);
            }

            _canHandleFlags = true;
        }

        private CaptureState TryCaptureFlag(FlagModel model, IFlagInvader invader)
        {
            var captureState = model.TryCapture();

            switch (captureState)
            {
                case CaptureState.Capturing:
                    TryStartMiniGame(model, invader);

                    // todo: capturing time scale?
                    break;

                case CaptureState.Captured:

                    bool isLastFlag = HandleFlagCaptured(invader);

                    if (isLastFlag)
                    {
                        return CaptureState.CapturedLastFlag;
                    }

                    break;

                case CaptureState.WaitingMiniGame:
                    break;
            }

            return captureState;
        }

        private List<FlagView> TryCaptureFlags()
        {
            List<FlagView> flagsToRemove = null;

            foreach (var invader in _invaders)
            {
                if (IsInvaderBlocked(invader))
                {
                    Debug.Log("is blocked");
                    continue;
                }

                foreach (var flag in _flags)
                {
                    float squaredDistance = (invader.GetPosition() - flag.Value.Position).sqrMagnitude;

                    if (squaredDistance <= _squaredRadius)
                    {
                        var captureState = TryCaptureFlag(flag.Value, invader);

                        if (captureState == CaptureState.CapturedLastFlag)
                        {
                            _canHandleFlags = false;

                            flagsToRemove ??= new List<FlagView>();
                            flagsToRemove.AddRange(_flags.Keys);
                        }
                        else if (captureState == CaptureState.Captured)
                        {
                            flagsToRemove ??= new List<FlagView>();
                            flagsToRemove.Add(flag.Key);
                        }
                    }
                }
            }

            return flagsToRemove;
        }

        private void TryStartMiniGame(FlagModel model, IFlagInvader invader)
        {
            if (model.CanStartMiniGame && Random.value < _miniGameChance)
            {
                model.WaitForMiniGame();
                _miniGameHandler.CallMiniGame(model, invader);
            }
        }
    }
}
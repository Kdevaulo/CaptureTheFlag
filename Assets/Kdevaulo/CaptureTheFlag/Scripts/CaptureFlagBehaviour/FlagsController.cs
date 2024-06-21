﻿using System.Collections.Generic;
using System.Linq;

using Mirror;

using UnityEngine;
using UnityEngine.Assertions;

namespace Kdevaulo.CaptureTheFlag.CaptureFlagBehaviour
{
    public class FlagsController : IUpdatable, IClearable, IClientDisconnectionHandler
    {
        private readonly FlagFactory _factory;
        private readonly IMiniGameHandler _miniGameHandler;
        private readonly IPlayerTuner _playerTuner;
        private readonly FlagSettings _settings;
        private Dictionary<IPlayer, float> _blockedInvaders;

        private bool _canHandleFlags;

        private Dictionary<FlagView, FlagModel> _flags;
        private Dictionary<IPlayer, int> _invadersCaptures;

        private List<IPlayer> _invaders;
        private List<FlagView> _flagsToRemove;
        private List<IPlayer> _invadersToRemove;

        private int _maxFlags;

        private float _miniGameChance;
        private float _squaredRadius;

        public FlagsController(FlagSettings settings, FlagFactory factory, IMiniGameHandler miniGameHandler,
            IPlayerTuner playerTuner)
        {
            _settings = settings;
            _factory = factory;
            _miniGameHandler = miniGameHandler;
            _playerTuner = playerTuner;

            _maxFlags = _settings.FlagsCount;
            _miniGameChance = _settings.MiniGameChance;
            _squaredRadius = _settings.FlagRadiusInUnits * _settings.FlagRadiusInUnits;

            _playerTuner.PlayerTuned += HandlePlayerTuned;
            _miniGameHandler.HandleMiniGameLost += HandleMiniGameLost;
        }

        void IClearable.Clear()
        {
            _canHandleFlags = false;

            _flags = null;
            _invaders = null;
            _invadersCaptures = null;
        }

        void IUpdatable.Update()
        {
            if (_canHandleFlags)
            {
                TryCaptureFlags();
                TryClearRedundantFlags();
                TryClearRedundantInvaders();
            }
        }

        private void TryClearRedundantFlags()
        {
            if (_flagsToRemove != null)
            {
                foreach (var flag in _flagsToRemove)
                {
                    _flags.Remove(flag);
                    NetworkServer.Destroy(flag.gameObject);
                }

                _flagsToRemove.Clear();
            }
        }

        private void TryClearRedundantInvaders()
        {
            if (_invadersToRemove != null)
            {
                foreach (var invader in _invadersToRemove)
                {
                    _invaders.Remove(invader);
                }

                _invadersToRemove.Clear();
            }
        }

        private void AddInvader(IPlayer player)
        {
            _invaders ??= new List<IPlayer>();
            _invadersCaptures ??= new Dictionary<IPlayer, int>();
            _blockedInvaders ??= new Dictionary<IPlayer, float>();

            _invaders.Add(player);
            _invadersCaptures.Add(player, 0);
        }

        private bool HandleFlagCaptured(IPlayer player)
        {
            return ++_invadersCaptures[player] == _maxFlags;
        }

        private void HandleMiniGameLost(IPlayer player)
        {
            Assert.IsFalse(_blockedInvaders.ContainsKey(player));

            _blockedInvaders.Add(player, _settings.OnLoseDelay);
            Debug.Log("Added to block");
        }

        [Server]
        private void HandlePlayerTuned(Color color, IPlayer player)
        {
            Spawn(color, player.GetOwner());
            AddInvader(player);
        }

        [Server]
        private bool IsInvaderBlocked(IPlayer player)
        {
            if (_blockedInvaders.TryGetValue(player, out float blockTimeLeft))
            {
                blockTimeLeft -= Time.deltaTime;

                if (blockTimeLeft <= 0)
                {
                    _blockedInvaders.Remove(player);

                    Debug.Log("Unblocked");

                    return false;
                }

                _blockedInvaders[player] = blockTimeLeft;

                return true;
            }

            return false;
        }

        [Server]
        private void SetupFlags(FlagView[] flags, GameObject owner)
        {
            _flags ??= new Dictionary<FlagView, FlagModel>();

            foreach (var flagView in flags)
            {
                NetworkServer.Spawn(flagView.gameObject, owner);

                var position = _settings.GetPosition();
                flagView.SetPosition(position);

                var model = new FlagModel(owner, position, _settings.SecondsToCapture)
                {
                    CanStartMiniGame = true
                };

                _flags.Add(flagView, model);
            }

            _canHandleFlags = true;
        }

        [Server]
        private void Spawn(Color color, GameObject owner)
        {
            var flagViews = _factory.Create(color);
            SetupFlags(flagViews, owner);
        }

        [Server]
        private CaptureState TryCaptureFlag(FlagModel model, IPlayer player)
        {
            var captureState = model.TryCapture();

            switch (captureState)
            {
                case CaptureState.Capturing:
                    TryStartMiniGame(model, player);

                    // todo: capturing time scale?
                    break;

                case CaptureState.Captured:

                    bool isLastFlag = HandleFlagCaptured(player);

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

        [Server]
        private void TryCaptureFlags()
        {
            foreach (var invader in _invaders)
            {
                if (IsInvaderBlocked(invader))
                {
                    continue;
                }

                var owner = invader.GetOwner();

                foreach (var flag in _flags)
                {
                    bool canCapture = flag.Value.CheckOwner(owner);

                    if (!canCapture)
                    {
                        continue;
                    }

                    float squaredDistance = (invader.GetPosition() - flag.Value.Position).sqrMagnitude;

                    if (squaredDistance <= _squaredRadius)
                    {
                        var captureState = TryCaptureFlag(flag.Value, invader);

                        if (captureState == CaptureState.CapturedLastFlag)
                        {
                            _canHandleFlags = false;

                            _flagsToRemove ??= new List<FlagView>();
                            _flagsToRemove.AddRange(_flags.Keys);

                            Debug.Log("GameFinished");
                        }
                        else if (captureState == CaptureState.Captured)
                        {
                            _flagsToRemove ??= new List<FlagView>();
                            _flagsToRemove.Add(flag.Key);
                        }
                    }
                }
            }
        }

        [Server]
        private void TryStartMiniGame(FlagModel model, IPlayer player)
        {
            if (model.CanStartMiniGame && Random.value < _miniGameChance)
            {
                model.WaitForMiniGame();
                _miniGameHandler.CallMiniGame(model, player);
            }
        }

        [Server]
        void IClientDisconnectionHandler.HandleClientDisconnected(int id)
        {
            _invadersToRemove ??= new List<IPlayer>();

            var targetInvader = _invaders.First(x => x.GetId() == id);

            _invadersToRemove.Add(targetInvader);
        }
    }
}
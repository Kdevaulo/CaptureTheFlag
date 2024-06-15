using System.Collections.Generic;

using Mirror;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.CaptureFlagBehaviour
{
    public class FlagsController : IUpdatable, IFlagSpawner, IFlagInvaderObserver, IReinitializable
    {
        private readonly FlagSettings _settings;
        private readonly FlagSpawner _spawner;

        private Dictionary<FlagView, FlagModel> _flags;
        private bool _flagsSettedUp;

        private List<IFlagInvader> _invaders;
        private Dictionary<IFlagInvader, int> _invadersCaptures;

        private int _maxFlags;
        private float _squaredRadius;

        public FlagsController(FlagSettings settings, FlagSpawner spawner)
        {
            _settings = settings;
            _spawner = spawner;

            _maxFlags = _settings.FlagsCount;
            _squaredRadius = _settings.FlagRadiusInUnits * _settings.FlagRadiusInUnits;
        }

        void IFlagInvaderObserver.AddInvaders(params IFlagInvader[] invaders)
        {
            _invaders ??= new List<IFlagInvader>();
            _invaders.AddRange(invaders);

            _invadersCaptures ??= new Dictionary<IFlagInvader, int>();

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
            _flagsSettedUp = false;

            _flags = null;
            _invaders = null;
            _invadersCaptures = null;
        }

        void IUpdatable.Update()
        {
            if (_flagsSettedUp)
            {
                List<FlagView> flagsToRemove = null;

                flagsToRemove = TryCaptureFlags(flagsToRemove);

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

        private void SetupFlags(FlagView[] flags)
        {
            _flags ??= new Dictionary<FlagView, FlagModel>();

            foreach (var flagView in flags)
            {
                var position = _settings.GetPosition();
                flagView.SetPosition(position);
                _flags.Add(flagView, new FlagModel(position, _settings.SecondsToCapture));
            }

            _flagsSettedUp = true;
        }

        private void Stop()
        {
            _flagsSettedUp = false;

            foreach (var flag in _flags)
            {
                _spawner.DestroyFlag(flag.Key);
            }

            _flags.Clear();
        }

        private List<FlagView> TryCaptureFlags(List<FlagView> flagsToRemove)
        {
            foreach (var flag in _flags)
            {
                foreach (var invader in _invaders)
                {
                    float squaredDistance = (invader.GetPosition() - flag.Value.Position).sqrMagnitude;

                    if (squaredDistance <= _squaredRadius)
                    {
                        if (flag.Value.TryCapture())
                        {
                            flagsToRemove ??= new List<FlagView>();

                            invader.HandleFlagCaptured();

                            if (++_invadersCaptures[invader] == _maxFlags)
                            {
                                Stop();
                                invader.HandleAllCaptured();
                                return null;
                            }

                            flagsToRemove.Add(flag.Key);
                        }
                    }
                }
            }

            return flagsToRemove;
        }
    }
}
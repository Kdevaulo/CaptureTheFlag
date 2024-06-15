using System.Collections.Generic;
using System.Linq;

using Mirror;

using UnityEngine;
using UnityEngine.Assertions;

namespace Kdevaulo.CaptureTheFlag.CaptureFlagBehaviour
{
    public class FlagSpawner
    {
        private readonly FlagView _viewPrefab;
        private readonly Transform _parent;
        private readonly FlagSettings _settings;

        private Dictionary<FlagView, NetworkIdentity> _viewsByPlayer;

        public FlagSpawner(FlagView viewPrefab, FlagSettings settings, Transform parent)
        {
            _viewPrefab = viewPrefab;
            _parent = parent;
            _settings = settings;

            _viewsByPlayer = new Dictionary<FlagView, NetworkIdentity>();
        }

        [Command]
        public void Spawn(Color color, NetworkIdentity netIdentity)
        {
            int count = _settings.FlagsCount;
            Debug.Log("Spawn");

            Assert.IsTrue(count <= _settings.SpawnPositions.Length,
                "Flags more then spawn positions, so there would be overlays");

            for (int i = 0; i < count; i++)
            {
                var view = Object.Instantiate(_viewPrefab, _parent);
                NetworkServer.Spawn(view.gameObject);
                view.SetPosition(_settings.GetPosition());
                view.SetColor(color);
                _viewsByPlayer.Add(view, netIdentity);
            }
        }

        [ServerCallback]
        public void Clear(NetworkIdentity netIdentity)
        {
            var itemsToDestroy = _viewsByPlayer.Where(x => x.Value == netIdentity).Select(item => item.Key).ToList();

            foreach (var item in itemsToDestroy)
            {
                NetworkServer.Destroy(item.gameObject);
            }
        }
    }
}
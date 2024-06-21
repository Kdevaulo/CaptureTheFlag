using Mirror;

using UnityEngine;
using UnityEngine.Assertions;

namespace Kdevaulo.CaptureTheFlag.CaptureFlagBehaviour
{
    public class FlagFactory
    {
        private readonly Transform _parent;
        private readonly FlagSettings _settings;
        private readonly FlagView _viewPrefab;

        public FlagFactory(FlagView viewPrefab, FlagSettings settings, Transform parent)
        {
            _viewPrefab = viewPrefab;
            _parent = parent;
            _settings = settings;
        }

        [Server]
        public FlagView[] Create(Color color)
        {
            int count = _settings.FlagsCount;

            Assert.IsTrue(count <= _settings.SpawnPositions.Length,
                "Flags count is more then spawn positions, so there would be overlays");

            var views = new FlagView[count];

            for (int i = 0; i < count; i++)
            {
                var view = Object.Instantiate(_viewPrefab, _parent);
                view.SetColor(color);
                views[i] = view;
            }

            return views;
        }
    }
}
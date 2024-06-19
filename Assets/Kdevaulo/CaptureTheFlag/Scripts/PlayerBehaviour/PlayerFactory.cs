using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.PlayerBehaviour
{
    public class PlayerFactory
    {
        private readonly Transform _parent;
        private readonly PlayerView _view;

        public PlayerFactory(PlayerView view, Transform parent)
        {
            _view = view;
            _parent = parent;
        }

        public PlayerView Create()
        {
            var playerView = Object.Instantiate(_view, _parent);
            playerView.SetPosition(Vector3.zero);

            return playerView;
        }
    }
}
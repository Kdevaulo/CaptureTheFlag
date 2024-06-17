using System.Collections.Generic;

using Mirror;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.PlayerBehaviour
{
    public class PlayerMovement
    {
        private readonly float _movementSensitivity;
        private readonly UserInputHandler _userInputHandler;

        private PlayerView _playerView;

        private Dictionary<NetworkIdentity, PlayerView> _playerViews;

        public PlayerMovement(UserInputHandler userInputHandler, PlayerSettings settings)
        {
            _userInputHandler = userInputHandler;
            _movementSensitivity = settings.PlayerMovementSensitivity;

            _playerViews = new Dictionary<NetworkIdentity, PlayerView>();

            _userInputHandler.MoveVertical += HandleVerticalMovement;
            _userInputHandler.MoveHorizontal += HandleHorizontalMovement;
        }

        public void SetPlayer(PlayerView playerView, NetworkIdentity identity)
        {
            _playerViews.Add(identity, playerView);
        }

        private void HandleHorizontalMovement(float offset)
        {
            var targetPosition = new Vector3(offset * _movementSensitivity, 0, 0);
            HandleMovement(NetworkClient.connection.identity, targetPosition);
        }

        private void HandleMovement(NetworkIdentity identity, Vector3 position)
        {
            var view = _playerViews[identity];

            view.SetPosition(position);
        }

        private void HandleVerticalMovement(float offset)
        {
            var targetPosition = new Vector3(0, 0, offset * _movementSensitivity);
            HandleMovement(NetworkClient.connection.identity, targetPosition);
        }
    }
}
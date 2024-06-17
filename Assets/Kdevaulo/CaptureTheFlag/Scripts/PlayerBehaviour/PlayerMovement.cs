using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.PlayerBehaviour
{
    public class PlayerMovement
    {
        private readonly float _movementSensitivity;
        private readonly UserInputHandler _userInputHandler;

        private PlayerView _playerView;

        public PlayerMovement(UserInputHandler userInputHandler, PlayerSettings settings)
        {
            _userInputHandler = userInputHandler;
            _movementSensitivity = settings.PlayerMovementSensitivity;

            _userInputHandler.MoveVertical += HandleVerticalMovement;
            _userInputHandler.MoveHorizontal += HandleHorizontalMovement;
        }

        public void SetPlayer(PlayerView playerView)
        {
            _playerView = playerView;
        }

        private void HandleHorizontalMovement(float offset)
        {
            _playerView.Move(new Vector3(offset * _movementSensitivity, 0, 0));
        }

        private void HandleVerticalMovement(float offset)
        {
            _playerView.Move(new Vector3(0, 0, offset * _movementSensitivity));
        }
    }
}
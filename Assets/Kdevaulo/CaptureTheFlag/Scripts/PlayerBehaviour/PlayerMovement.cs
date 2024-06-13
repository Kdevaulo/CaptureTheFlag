using System;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.PlayerBehaviour
{
    public class PlayerMovement
    {
        private readonly UserInput _userInput;

        private readonly float _movementSensitivity;

        private PlayerView _playerView;

        public PlayerMovement(UserInput userInput, PlayerSettings settings)
        {
            _userInput = userInput;
            _movementSensitivity = settings.PlayerMovementSensitivity;

            _userInput.MoveVertical += HandleVerticalMovement;
            _userInput.MoveHorizontal += HandleHorizontalMovement;
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
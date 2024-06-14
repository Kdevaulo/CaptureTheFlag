using System;

using Mirror;

using UnityEngine;
using UnityEngine.Assertions;

namespace Kdevaulo.CaptureTheFlag.PlayerBehaviour
{
    [RequireComponent(typeof(PlayerView))]
    [AddComponentMenu(nameof(TempMovement) + " in " + nameof(PlayerBehaviour))]
    public class TempMovement : NetworkBehaviour
    {
        [Min(0)]
        [SerializeField] private float _movementSensitivity;

        private UserInput _userInput;
        private PlayerView _playerView;

        private void Awake()
        {
            _playerView = GetComponent<PlayerView>();

            _userInput = FindObjectOfType<UserInput>();
            Assert.IsNotNull(_userInput);

            _userInput.MoveVertical += HandleVerticalMovement;
            _userInput.MoveHorizontal += HandleHorizontalMovement;
        }

        private void HandleVerticalMovement(float offset)
        {
            if (isLocalPlayer)
            {
                _playerView.Move(new Vector3(0, 0, offset * _movementSensitivity));
            }
        }

        private void HandleHorizontalMovement(float offset)
        {
            if (isLocalPlayer)
            {
                _playerView.Move(new Vector3(offset * _movementSensitivity, 0, 0));
            }
        }
    }
}
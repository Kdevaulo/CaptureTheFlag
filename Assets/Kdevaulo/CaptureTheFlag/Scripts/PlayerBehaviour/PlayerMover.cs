using Mirror;

using UnityEngine.Assertions;

namespace Kdevaulo.CaptureTheFlag.PlayerBehaviour
{
    public class PlayerMover
    {
        private readonly float _movementSensitivity;

        public PlayerMover(PlayerSettings settings)
        {
            _movementSensitivity = settings.PlayerMovementSensitivity;
        }

        [Server]
        public void HandlePlayerMovement(PlayerView player, float moveHorizontal, float moveVertical)
        {
            Assert.IsNotNull(player);

            player.HandleMovement(moveHorizontal * _movementSensitivity, moveVertical * _movementSensitivity);
        }
    }
}
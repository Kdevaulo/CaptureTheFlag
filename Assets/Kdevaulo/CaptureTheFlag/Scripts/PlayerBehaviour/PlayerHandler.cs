using Mirror;

namespace Kdevaulo.CaptureTheFlag.PlayerBehaviour
{
    public class PlayerHandler
    {
        private readonly PlayerPool _pool;
        private readonly PlayerMovement _playerMovement;
        private readonly INetworkHandler _networkHandler;

        public PlayerHandler(INetworkHandler networkHandler, PlayerMovement playerMovement, PlayerPool pool)
        {
            _pool = pool;
            _networkHandler = networkHandler;
            _playerMovement = playerMovement;

            _networkHandler.ClientConnected += HandleClientConnected;
        }

        private void HandleClientConnected(NetworkConnectionToClient connection)
        {
            var playerView = _pool.Get();
            _playerMovement.SetPlayer(playerView);
            
            NetworkServer.AddPlayerForConnection(connection, playerView.gameObject);
        }
    }
}
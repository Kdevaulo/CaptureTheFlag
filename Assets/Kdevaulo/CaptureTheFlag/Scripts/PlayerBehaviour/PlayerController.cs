using Mirror;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.PlayerBehaviour
{
    public class PlayerController
    {
        private readonly IColorProvider _colorProvider;

        private readonly PlayerFactory _factory;
        private readonly IFlagSpawner _flagSpawner;
        private readonly INetworkHandler _networkHandler;
        private readonly IFlagInvaderObserver _observer;
        private readonly PlayerMovement _playerMovement;

        public PlayerController(INetworkHandler networkHandler, PlayerMovement playerMovement, PlayerFactory factory,
            IColorProvider colorProvider, IFlagSpawner flagSpawner, IFlagInvaderObserver observer)
        {
            _factory = factory;
            _playerMovement = playerMovement;

            _flagSpawner = flagSpawner;
            _observer = observer;
            _colorProvider = colorProvider;
            _networkHandler = networkHandler;

            _networkHandler.ClientConnected += HandleClientConnected;
        }

        private void HandleClientConnected(NetworkConnectionToClient connection)
        {
            Debug.Log("Connected");
            var color = _colorProvider.GetColor();

            var view = _factory.Create();
            NetworkServer.AddPlayerForConnection(connection, view.gameObject);

            view.SetColor(color);

            _observer.AddInvaders(view);

            _flagSpawner.Spawn(color, view.GetNetIdentity());

            _playerMovement.SetPlayer(view, connection.identity);
        }
    }
}

using Mirror;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.PlayerBehaviour
{
    public class PlayerHandler
    {
        private readonly INetworkHandler _networkHandler;

        private readonly PlayerFactory _factory;
        private readonly PlayerSettings _settings;
        private readonly PlayerMovement _playerMovement;

        public PlayerHandler(INetworkHandler networkHandler, /*PlayerMovement playerMovement,*/ /*PlayerFactory factory,*/
            PlayerSettings settings)
        {
            _networkHandler = networkHandler;
            //_playerMovement = playerMovement;
            //_factory = factory;
            _settings = settings;

            _networkHandler.ClientConnected += HandleClientConnected;
        }

        private void HandleClientConnected(NetworkConnectionToClient connection)
        {
            
            Debug.Log("Connected");
            //var view = _factory.Create(_settings.GetColor());
            //_playerMovement.SetPlayer(view);

            //NetworkServer.AddPlayerForConnection(connection, view.gameObject);
        }
    }
}
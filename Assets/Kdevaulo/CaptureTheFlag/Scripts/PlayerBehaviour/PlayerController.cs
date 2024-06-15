using Mirror;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.PlayerBehaviour
{
    public class PlayerController : IColorGetter
    {
        private readonly PlayerFactory _factory;

        private readonly INetworkHandler _networkHandler;
        private readonly PlayerMovement _playerMovement;
        private readonly PlayerSettings _settings;

        public PlayerController(
            INetworkHandler networkHandler, /*PlayerMovement playerMovement,*/ /*PlayerFactory factory,*/
            PlayerSettings settings)
        {
            _networkHandler = networkHandler;
            //_playerMovement = playerMovement;
            //_factory = factory;
            _settings = settings;

            _networkHandler.ClientConnected += HandleClientConnected;
        }

        Color IColorGetter.GetColor()
        {
            return _settings.GetColor();
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
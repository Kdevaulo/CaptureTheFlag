using System;
using System.Linq;

using Mirror;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.Networking
{
    [AddComponentMenu(nameof(NetworkBehaviourHandler) + " in " + nameof(Networking))]
    public class NetworkBehaviourHandler : NetworkManager
    {
        public event Action ClientDisconnected = delegate { };
        public event Action<int> ClientDisconnectedFromServer = delegate { };

        private IPlayerTuner _playerTuner;
        private IPlayerProvider _playerProvider;
        private IPlayerMovementHandler _movementHandler;
        private IMiniGameHandler _miniGameHandler;

        public void Initialize(IPlayerProvider playerProvider, IPlayerMovementHandler movementHandler,
            IPlayerTuner playerTuner, IMiniGameHandler miniGameHandler)
        {
            _miniGameHandler = miniGameHandler;

            _playerTuner = playerTuner;
            _playerProvider = playerProvider;
            _movementHandler = movementHandler;

            _miniGameHandler.HandleMiniGameLost += CallLostMessage;
        }

        [Server]
        public override void OnStartServer()
        {
            NetworkServer.RegisterHandler<ClientConnectedMessage>(HandleClientConnection);
        }

        [Client]
        public override void OnClientConnect()
        {
            NetworkClient.Ready();

            var connection = NetworkClient.connection;
            connection.Send(new ClientConnectedMessage());
        }

        /// <summary>
        /// Works on Server
        /// </summary>
        public override void OnServerDisconnect(NetworkConnectionToClient connection)
        {
            base.OnServerDisconnect(connection);

            NetworkServer.DestroyPlayerForConnection(connection);

            ClientDisconnectedFromServer.Invoke(connection.connectionId);
        }

        /// <summary>
        /// Works on Client
        /// </summary>
        public override void OnClientDisconnect()
        {
            ClientDisconnected.Invoke();
        }

        [Server]
        private void HandleClientConnection(NetworkConnectionToClient connection, ClientConnectedMessage message)
        {
            var player = _playerProvider.SpawnPlayer(connection.connectionId);

            NetworkServer.AddPlayerForConnection(connection, player.gameObject);

            _playerTuner.TunePlayer(player);

            player.Initialize(connection, _movementHandler, _miniGameHandler);
        }

        [Server]
        private void CallLostMessage(IPlayer player)
        {
            int connectionId = player.GetId();

            var message = new MiniGameLoseMessage
            {
                Message = LocalizationStrings.LoseMessage
            };
            
            foreach (var item in NetworkServer.connections.Where(x => x.Key != connectionId))
            {
                item.Value.Send(message);
            }
        }
    }
}
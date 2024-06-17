using System;
using System.Linq;

using Mirror;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.Networking
{
    [AddComponentMenu(nameof(NetworkBehaviourHandler) + " in " + nameof(Networking))]
    public class NetworkBehaviourHandler : NetworkManager, INetworkHandler
    {
        public event Action<NetworkConnectionToClient> ClientConnected = delegate { };
        public event Action<NetworkConnectionToClient> ClientDisconnects = delegate { };

        public override void OnStartServer()
        {
            NetworkServer.RegisterHandler<ClientConnectedMessage>(HandleClientConnection);
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();

            var message = new ClientConnectedMessage();
            NetworkClient.Send(message);
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);

            ClientDisconnects.Invoke(conn);
        }

        public void SetMessageCaller(IMiniGameLostHandler handler)
        {
            handler.HandleMiniGameLost += CallLostMessage;
        }

        private void HandleClientConnection(NetworkConnectionToClient connection, ClientConnectedMessage message)
        {
            ClientConnected.Invoke(connection);
            Debug.Log(
                $"Player connected - Id = {message.Id}");
        }

        private void CallLostMessage(IFlagInvader invader)
        {
            var identity = invader.GetNetIdentity();

            var message = new MiniGameLoseMessage()
            {
                Message = LocalizationStrings.LoseMessage,
                Identity = identity
            };

            foreach (var item in NetworkServer.connections.Where(x => x.Value.identity != identity))
            {
                item.Value.Send(message);
            }
        }
    }
}
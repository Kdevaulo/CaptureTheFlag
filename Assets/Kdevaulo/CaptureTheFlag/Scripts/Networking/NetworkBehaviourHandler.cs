using System;

using Mirror;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.Networking
{
    [AddComponentMenu(nameof(NetworkBehaviourHandler) + " in " + nameof(Networking))]
    public class NetworkBehaviourHandler : NetworkManager, INetworkHandler
    {
        public event Action<NetworkConnectionToClient> ClientConnected = delegate { };

        public override void OnStartServer()
        {
            NetworkServer.RegisterHandler<CharacterCreatedMessage>(HandleClientConnection);
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();

            var message = new CharacterCreatedMessage();
            NetworkClient.Send(message);
        }

        private void HandleClientConnection(NetworkConnectionToClient connection, CharacterCreatedMessage message,
            int channelId)
        {
            ClientConnected.Invoke(connection);
            Debug.Log(
                
                $"Player connected - Id = {message.Id}");
        }
    }
}
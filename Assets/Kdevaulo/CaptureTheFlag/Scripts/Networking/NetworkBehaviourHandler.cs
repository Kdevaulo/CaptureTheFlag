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
            var characterMessage = new CharacterCreatedMessage
            {
                Id = 0,
                PlayerName = "Test",
                CharacterColor = Color.cyan
            };

            Debug.Log("Client Connected");

            NetworkClient.Send(characterMessage);
        }

        private void HandleClientConnection(NetworkConnectionToClient connection, CharacterCreatedMessage message,
            int channelId)
        {
            Debug.Log(
                $"Player connected - Id = {message.Id}, Name = {message.PlayerName}, Color = {message.CharacterColor}");

            ClientConnected.Invoke(connection);
        }
    }
}
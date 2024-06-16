using Mirror;

namespace Kdevaulo.CaptureTheFlag.Networking
{
    public struct MiniGameLoseMessage : NetworkMessage
    {
        public NetworkIdentity Identity;
        public string Message;
    }
}
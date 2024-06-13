using System;

using Mirror;

namespace Kdevaulo.CaptureTheFlag
{
    public interface INetworkHandler
    {
        event Action<NetworkConnectionToClient> ClientConnected;
    }
}
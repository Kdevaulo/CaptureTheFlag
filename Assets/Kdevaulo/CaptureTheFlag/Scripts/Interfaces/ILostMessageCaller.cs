using System;

using Mirror;

namespace Kdevaulo.CaptureTheFlag
{
    public interface ILostMessageCaller
    {
        event Action<NetworkIdentity> CallLostMessage;
    }
}
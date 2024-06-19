using System;

namespace Kdevaulo.CaptureTheFlag
{
    public interface IMiniGameLostHandler
    {
        event Action<IPlayer> HandleMiniGameLost;
    }
}
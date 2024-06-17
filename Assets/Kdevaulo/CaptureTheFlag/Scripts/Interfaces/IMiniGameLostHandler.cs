using System;


namespace Kdevaulo.CaptureTheFlag
{
    public interface IMiniGameLostHandler
    {
        event Action<IFlagInvader> HandleMiniGameLost;
    }
}
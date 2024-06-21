using System;

namespace Kdevaulo.CaptureTheFlag
{
    public interface IMiniGameHandler : IMiniGameClientInitializer, IMiniGameEventsHandler
    {
        event Action<IPlayer> HandleMiniGameLost;
        void CallMiniGame(IMiniGameObserver observer, IPlayer player);
        
    }
}
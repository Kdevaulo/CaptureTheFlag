namespace Kdevaulo.CaptureTheFlag
{
    public interface IMiniGameHandler : IMiniGameLostHandler
    {
        void CallMiniGame(IMiniGameObserver observer, IFlagInvader invader);
    }
}
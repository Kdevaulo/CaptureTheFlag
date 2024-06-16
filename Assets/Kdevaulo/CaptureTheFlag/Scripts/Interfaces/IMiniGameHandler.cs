using Mirror;

namespace Kdevaulo.CaptureTheFlag
{
    public interface IMiniGameHandler
    {
        void CallMiniGame(IMiniGameObserver observer, NetworkIdentity identity);
    }
}
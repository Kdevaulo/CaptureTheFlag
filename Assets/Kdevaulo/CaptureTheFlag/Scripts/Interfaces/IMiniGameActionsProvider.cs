using Kdevaulo.CaptureTheFlag.MiniGameBehaviour;

namespace Kdevaulo.CaptureTheFlag
{
    public interface IMiniGameActionsProvider
    {
        void InitializeMiniGame(int id, MiniGameData data);
        void CmdSendEvents(bool isCorrectAction, string guid);
    }
}
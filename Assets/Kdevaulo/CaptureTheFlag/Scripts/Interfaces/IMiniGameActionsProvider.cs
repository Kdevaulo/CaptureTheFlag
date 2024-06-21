using Kdevaulo.CaptureTheFlag.MiniGameBehaviour;

namespace Kdevaulo.CaptureTheFlag
{
    public interface IMiniGameActionsProvider
    {
        void CmdSendEvents(bool isCorrectAction, string guid);
        void InitializeMiniGame(int id, MiniGameData data);
    }
}
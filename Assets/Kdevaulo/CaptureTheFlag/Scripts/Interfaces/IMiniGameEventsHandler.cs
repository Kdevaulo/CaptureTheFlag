namespace Kdevaulo.CaptureTheFlag
{
    public interface IMiniGameEventsHandler
    {
        void SendEvents(bool isCorrectAction, string guid);
        void SetActionsProvider(IMiniGameActionsProvider actionsProvider);
    }
}
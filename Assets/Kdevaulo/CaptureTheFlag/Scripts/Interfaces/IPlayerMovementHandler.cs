namespace Kdevaulo.CaptureTheFlag
{
    public interface IPlayerMovementHandler
    {
        void TryMovePlayer(int connectionId, float moveHorizontal, float moveVertical);
    }
}
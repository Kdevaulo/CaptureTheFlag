namespace Kdevaulo.CaptureTheFlag
{
    public interface IFlagInvaderObserver
    {
        void AddInvaders(params IFlagInvader[] invaders);
    }
}
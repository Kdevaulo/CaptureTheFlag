namespace Kdevaulo.CaptureTheFlag
{
    public interface IClientDisconnectionHandler
    {
        void HandleClientDisconnected(int id);
    }
}
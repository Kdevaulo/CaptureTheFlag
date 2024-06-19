using Kdevaulo.CaptureTheFlag.PlayerBehaviour;

namespace Kdevaulo.CaptureTheFlag
{
    public interface IPlayerProvider
    {
        PlayerView SpawnPlayer(int id);
    }
}
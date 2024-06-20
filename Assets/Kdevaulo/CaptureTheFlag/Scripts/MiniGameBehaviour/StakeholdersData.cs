namespace Kdevaulo.CaptureTheFlag.MiniGameBehaviour
{
    public class StakeholdersData
    {
        public IMiniGameObserver Observer { get; private set; }
        public IPlayer Player { get; private set; }

        public StakeholdersData(IMiniGameObserver observer, IPlayer player)
        {
            Observer = observer;
            Player = player;
        }
    }
}
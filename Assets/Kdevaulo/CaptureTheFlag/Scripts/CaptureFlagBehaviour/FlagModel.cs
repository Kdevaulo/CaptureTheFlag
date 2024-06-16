using Kdevaulo.CaptureTheFlag.MiniGameBehaviour;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.CaptureFlagBehaviour
{
    public class FlagModel : IMiniGameObserver
    {
        public bool CanStartMiniGame { get; set; }

        public Vector3 Position { get; private set; }

        private readonly float _blockSeconds;

        private float _blockTimeLeft;

        private bool _canCapture;
        private float _secondsToCapture;
        private bool _waitingForMiniGame;

        public FlagModel(Vector3 position, float secondsToCapture, float blockSeconds)
        {
            Position = position;

            _secondsToCapture = secondsToCapture;
            _blockSeconds = blockSeconds;

            _waitingForMiniGame = false;
        }

        void IMiniGameObserver.HandleMiniGameFinished(MiniGameState state)
        {
            if (state == MiniGameState.Lose)
            {
                Block(_blockSeconds);
            }

            CanStartMiniGame = false;
            _waitingForMiniGame = false;
        }

        public CaptureState TryCapture()
        {
            if (_waitingForMiniGame)
            {
                return CaptureState.WaitingMiniGame;
            }

            if (_blockTimeLeft <= 0)
            {
                _canCapture = true;
            }

            if (_canCapture)
            {
                _secondsToCapture -= Time.deltaTime;
                return _secondsToCapture <= 0 ? CaptureState.Captured : CaptureState.Capturing;
            }

            _blockTimeLeft -= Time.deltaTime;
            return CaptureState.Blocked;
        }

        public void WaitForMiniGame()
        {
            _waitingForMiniGame = true;
        }

        private void Block(float timeInSeconds)
        {
            _canCapture = false;
            _blockTimeLeft = timeInSeconds;
        }
    }
}
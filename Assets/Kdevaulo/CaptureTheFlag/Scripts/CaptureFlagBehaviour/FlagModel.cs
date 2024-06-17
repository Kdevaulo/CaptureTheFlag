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

        public FlagModel(Vector3 position, float secondsToCapture)
        {
            Position = position;

            _secondsToCapture = secondsToCapture;

            _waitingForMiniGame = false;
        }

        void IMiniGameObserver.HandleMiniGameFinished()
        {
            CanStartMiniGame = true;
            _waitingForMiniGame = false;
        }

        public CaptureState TryCapture()
        {
            if (_waitingForMiniGame)
            {
                return CaptureState.WaitingMiniGame;
            }

            _secondsToCapture -= Time.deltaTime;
            return _secondsToCapture <= 0 ? CaptureState.Captured : CaptureState.Capturing;
        }

        public void WaitForMiniGame()
        {
            _waitingForMiniGame = true;
        }
    }
}
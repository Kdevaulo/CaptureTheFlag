using Mirror;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.CaptureFlagBehaviour
{
    public class FlagModel : IMiniGameObserver
    {
        public bool CanStartMiniGame { get; set; }

        public Vector3 Position { get; private set; }

        private readonly float _blockSeconds;
        private readonly GameObject _owner;

        private float _blockTimeLeft;
        private float _secondsToCapture;

        private bool _canCapture;
        private bool _waitingForMiniGame;

        public FlagModel(GameObject owner, Vector3 position, float secondsToCapture)
        {
            Position = position;

            _owner = owner;
            _secondsToCapture = secondsToCapture;

            _waitingForMiniGame = false;
        }

        [Server]
        void IMiniGameObserver.HandleMiniGameFinished()
        {
            CanStartMiniGame = true;
            _waitingForMiniGame = false;
        }

        [Server]
        public bool CheckOwner(GameObject target)
        {
            return target == _owner;
        }

        [Server]
        public CaptureState TryCapture()
        {
            if (_waitingForMiniGame)
            {
                return CaptureState.WaitingMiniGame;
            }

            _secondsToCapture -= Time.deltaTime;
            return _secondsToCapture <= 0 ? CaptureState.Captured : CaptureState.Capturing;
        }

        [Server]
        public void WaitForMiniGame()
        {
            _waitingForMiniGame = true;
        }
    }
}
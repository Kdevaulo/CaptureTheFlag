using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.CaptureFlagBehaviour
{
    public class FlagModel
    {
        public Vector3 Position { get; private set; }

        private float _blockTimeLeft;

        private bool _canCapture;
        private float _secondsToCapture;

        public FlagModel(Vector3 position, float secondsToCapture)
        {
            _secondsToCapture = secondsToCapture;
            Position = position;
        }

        public void Block(float timeInSeconds)
        {
            _canCapture = false;
            _blockTimeLeft = timeInSeconds;
        }

        public bool TryCapture()
        {
            if (_blockTimeLeft <= 0)
            {
                _canCapture = true;
            }

            if (_canCapture)
            {
                _secondsToCapture -= Time.deltaTime;

                return _secondsToCapture <= 0;
            }

            _blockTimeLeft -= Time.deltaTime;
            return false;
        }
    }
}
using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.CaptureFlagBehaviour
{
    public class PositionData
    {
        public bool IsBusy;
        public Vector3 Position;

        public PositionData(Vector3 position, bool isBusy)
        {
            Position = position;
            IsBusy = isBusy;
        }
    }
}
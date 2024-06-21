using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.CaptureFlagBehaviour
{
    public class PositionData
    {
        public Vector3 Position;
        public bool IsBusy;

        public PositionData(Vector3 position, bool isBusy)
        {
            Position = position;
            IsBusy = isBusy;
        }
    }
}
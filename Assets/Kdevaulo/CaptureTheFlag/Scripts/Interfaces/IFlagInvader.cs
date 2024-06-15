using UnityEngine;

namespace Kdevaulo.CaptureTheFlag
{
    public interface IFlagInvader
    {
        Vector3 GetPosition();
        void HandleAllCaptured();
        void HandleFlagCaptured();
    }
}
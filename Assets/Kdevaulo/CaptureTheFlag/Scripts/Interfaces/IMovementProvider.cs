using System;

namespace Kdevaulo.CaptureTheFlag
{
    public interface IMovementProvider
    {
        event Action<float> MoveHorizontal;
        event Action<float> MoveVertical;
    }
}
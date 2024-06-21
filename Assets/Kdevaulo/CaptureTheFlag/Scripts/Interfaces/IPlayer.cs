using Kdevaulo.CaptureTheFlag.MiniGameBehaviour;

using Mirror;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag
{
    public interface IPlayer : IMiniGameActionsProvider
    {
        GameObject GetOwner();
        int GetId();
        Vector3 GetPosition();
    }
}
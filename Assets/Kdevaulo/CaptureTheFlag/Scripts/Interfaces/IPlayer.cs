using UnityEngine;

namespace Kdevaulo.CaptureTheFlag
{
    public interface IPlayer : IMiniGameActionsProvider
    {
        int GetId();
        GameObject GetOwner();
        Vector3 GetPosition();
    }
}
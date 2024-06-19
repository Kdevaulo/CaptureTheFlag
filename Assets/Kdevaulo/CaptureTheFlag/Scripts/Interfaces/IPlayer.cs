using Mirror;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag
{
    public interface IPlayer
    {
        GameObject GetOwner();
        Vector3 GetPosition();
    }
}
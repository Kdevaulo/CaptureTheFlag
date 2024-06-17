using Mirror;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag
{
    public interface IFlagInvader
    {
        NetworkIdentity GetNetIdentity();
        Vector3 GetPosition();
    }
}
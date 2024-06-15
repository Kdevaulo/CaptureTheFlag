using Mirror;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag
{
    public interface IFlagSpawner
    {
        [ServerCallback]
        void Clear(NetworkIdentity netIdentity);

        [Command]
        void Spawn(Color color, NetworkIdentity netIdentity);
    }
}
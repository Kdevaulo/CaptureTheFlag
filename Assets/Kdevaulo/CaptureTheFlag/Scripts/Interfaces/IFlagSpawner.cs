using Mirror;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag
{
    public interface IFlagSpawner
    {
        void Clear(NetworkIdentity netIdentity);

        void Spawn(Color color, NetworkIdentity netIdentity);
    }
}
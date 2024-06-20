using Kdevaulo.CaptureTheFlag.MiniGameBehaviour;

using Mirror;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag
{
    public interface IPlayer
    {
        GameObject GetOwner();
        int GetId();
        Vector3 GetPosition();
        void InitializeMiniGame(int id, MiniGameData data);
    }
}
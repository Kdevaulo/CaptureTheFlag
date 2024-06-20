using System;

using Kdevaulo.CaptureTheFlag.MiniGameBehaviour;

using Mirror;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag
{
    [AddComponentMenu(nameof(MonoBehaviourProvider) + " in " + nameof(CaptureTheFlag))]
    public class MonoBehaviourProvider : MonoBehaviour
    {
        public event Action MovableSet = delegate { };

        public IMovable Movable { get; private set; }

        private IMiniGameClientInitializer _initializer;

        public void Initialize(IMiniGameClientInitializer initializer)
        {
            _initializer = initializer;
        }

        public void SetMovable(IMovable movable)
        {
            Movable = movable;

            MovableSet.Invoke();
        }

        [Client]
        public void InitializeMiniGame(MiniGameData data)
        {
            _initializer.InitializeMiniGame(data);
        }
    }
}
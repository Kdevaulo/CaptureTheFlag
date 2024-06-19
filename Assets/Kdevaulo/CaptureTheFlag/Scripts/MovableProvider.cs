using System;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag
{
    [AddComponentMenu(nameof(MovableProvider) + " in " + nameof(CaptureTheFlag))]
    public class MovableProvider : MonoBehaviour
    {
        public event Action MovableSet = delegate { };

        public IMovable Movable { get; private set; }

        public void SetMovable(IMovable movable)
        {
            Movable = movable;

            MovableSet.Invoke();
        }
    }
}
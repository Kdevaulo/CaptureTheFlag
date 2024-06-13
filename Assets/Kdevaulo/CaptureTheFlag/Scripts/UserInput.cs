using System;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag
{
    [AddComponentMenu(nameof(UserInput) + " in " + nameof(CaptureTheFlag))]
    public class UserInput : MonoBehaviour, IUpdatable
    {
        public event Action<float> MoveVertical = delegate { };
        public event Action<float> MoveHorizontal = delegate { };

        [SerializeField] private Joystick _joystick;

        void IUpdatable.Update()
        {
            if (_joystick.Vertical != 0)
            {
                MoveVertical.Invoke(_joystick.Vertical * Time.deltaTime);
            }

            if (_joystick.Horizontal != 0)
            {
                MoveHorizontal.Invoke(_joystick.Horizontal * Time.deltaTime);
            }
        }
    }
}
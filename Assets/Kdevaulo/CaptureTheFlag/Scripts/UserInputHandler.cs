using System;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag
{
    public class UserInputHandler : IUpdatable, IPauseHandler
    {
        public event Action<float> MoveHorizontal = delegate { };
        public event Action<float> MoveVertical = delegate { };

        private readonly Joystick _joystick;

        private bool _isPaused;

        public UserInputHandler(Joystick joystick)
        {
            _joystick = joystick;
        }

        void IPauseHandler.HandlePause()
        {
            _isPaused = true;
        }

        void IPauseHandler.HandleResume()
        {
            _isPaused = false;
        }

        void IUpdatable.Update()
        {
            if (_isPaused)
            {
                return;
            }

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
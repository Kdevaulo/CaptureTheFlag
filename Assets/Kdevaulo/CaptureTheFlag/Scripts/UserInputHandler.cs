using System;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag
{
    public class UserInputHandler : IUpdatable, IPauseHandler, IMovementProvider
    {
        public event Action<float> MoveHorizontal = delegate { };
        public event Action<float> MoveVertical = delegate { };

        private readonly Joystick _joystick;

        private bool _canHandle = true;

        public UserInputHandler(Joystick joystick)
        {
            _joystick = joystick;
        }

        void IPauseHandler.HandlePause()
        {
            _canHandle = true;
        }

        void IPauseHandler.HandleResume()
        {
            _canHandle = false;
        }

        void IUpdatable.Update()
        {
            if (!_canHandle)
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
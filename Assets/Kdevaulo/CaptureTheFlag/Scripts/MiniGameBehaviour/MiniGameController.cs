using System;

using Mirror;

using Random = UnityEngine.Random;

namespace Kdevaulo.CaptureTheFlag.MiniGameBehaviour
{
    public class MiniGameController : IUpdatable, IMiniGameHandler, ILostMessageCaller
    {
        public event Action<NetworkIdentity> CallLostMessage = delegate { };

        private readonly MiniGameSettings _settings;
        private readonly MiniGameView _view;

        private bool _canMove;

        private float _movementSpeed;

        public MiniGameController(MiniGameView view, MiniGameSettings settings)
        {
            _view = view;
            _settings = settings;
            _movementSpeed = _settings.MovementSpeed;
        }

        void IMiniGameHandler.CallMiniGame(IMiniGameObserver observer, NetworkIdentity identity)
        {
            _view.SetCorrectAreaPosition(Random.value);
            _canMove = true;
            _view.Enable();

            // todo: game logic

            observer.HandleMiniGameFinished(MiniGameState.Lose);
            _view.Disable();
            _canMove = false;
            CallLostMessage.Invoke(identity);
        }

        void IUpdatable.Update()
        {
            if (_canMove)
            {
                _view.MoveFlag(_movementSpeed);
            }
        }
    }
}
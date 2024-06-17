using System;
using System.Collections.Generic;

using Mirror;

using UnityEngine;

using Random = UnityEngine.Random;
using Time = UnityEngine.Time;

namespace Kdevaulo.CaptureTheFlag.MiniGameBehaviour
{
    public class MiniGameController : IUpdatable, IMiniGameHandler
    {
        public event Action<IFlagInvader> HandleMiniGameLost = delegate { };

        private readonly MiniGameSettings _settings;
        private readonly MiniGameView _view;

        private readonly IPauseHandler[] _pauseHandlers;

        private Dictionary<MiniGameView, MiniGameModel> _activeGames;

        public MiniGameController(MiniGameView view, MiniGameSettings settings, params IPauseHandler[] pauseHandlers)
        {
            _view = view;
            _settings = settings;
            _pauseHandlers = pauseHandlers;

            _activeGames = new Dictionary<MiniGameView, MiniGameModel>();

            _view.Clicked += HandleClick;
            _view.Disable();
        }

        void IMiniGameHandler.CallMiniGame(IMiniGameObserver observer, IFlagInvader invader)
        {
            if (NetworkClient.connection.identity != invader.GetNetIdentity())
            {
                return;
            }

            float correctPosition = Random.value;
            _view.SetCorrectAreaPosition(correctPosition);
            _view.Enable();

            var model = new MiniGameModel(observer, invader, _settings.GameDurationInSeconds, correctPosition,
                _settings.MovementSpeed, _view.GetCorrectAreaSize());

            _activeGames.Add(_view, model);

            SetPauseState(true);
        }

        void IUpdatable.Update()
        {
            MiniGameView itemToRemove = null;

            foreach (var pair in _activeGames)
            {
                var model = pair.Value;
                var view = pair.Key;

                model.Move(Time.deltaTime);
                view.SetFlagPosition(model.Position);

                if (model.IsTimeOver())
                {
                    itemToRemove = view;
                    FinishGame(false, view, model);
                }
            }

            if (itemToRemove != null)
            {
                _activeGames.Remove(itemToRemove);
            }
        }

        private void HandleClick(MiniGameView view)
        {
            var model = _activeGames[view];
            _activeGames.Remove(view);

            bool correctClick = model.CheckPosition();

            FinishGame(correctClick, view, model);
        }

        private void FinishGame(bool correctAction, MiniGameView view, MiniGameModel model)
        {
            if (correctAction)
            {
                Debug.Log("CorrectAction");
            }
            else
            {
                Debug.Log("IncorrectAction");
                HandleMiniGameLost.Invoke(model.Invader);
            }

            model.Observer.HandleMiniGameFinished();

            view.Disable();

            SetPauseState(false);
        }

        private void SetPauseState(bool state)
        {
            foreach (var handler in _pauseHandlers)
            {
                if (state)
                {
                    handler.HandlePause();
                }
                else
                {
                    handler.HandleResume();
                }
            }
        }
    }
}
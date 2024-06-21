using System;
using System.Collections.Generic;

using Mirror;

using UnityEngine;
using UnityEngine.Assertions;

using Random = UnityEngine.Random;

namespace Kdevaulo.CaptureTheFlag.MiniGameBehaviour
{
    public class MiniGameController : IUpdatable, IMiniGameHandler
    {
        public event Action<IPlayer> HandleMiniGameLost = delegate { };

        private readonly IPauseHandler[] _pauseHandlers;

        private readonly MiniGameSettings _settings;
        private readonly MiniGameView _view;

        private IMiniGameActionsProvider _actionsProvider;

        private Dictionary<string, StakeholdersData> _stakeholdersByIds;

        private MiniGameModel _model;

        private bool _initialized;

        public MiniGameController(MiniGameView view, MiniGameSettings settings, params IPauseHandler[] pauseHandlers)
        {
            _view = view;
            _settings = settings;
            _pauseHandlers = pauseHandlers;

            _stakeholdersByIds = new Dictionary<string, StakeholdersData>();

            _view.Clicked += HandleClick;
            _view.Disable();
        }

        [Client]
        void IMiniGameClientInitializer.InitializeMiniGame(MiniGameData data)
        {
            _model = new MiniGameModel(data);

            _view.SetCorrectAreaPosition(data.CorrectPosition);
            _view.SetFlagPosition(0);
            _view.Enable();

            SetPauseState(true);

            _initialized = true;
        }

        void IMiniGameEventsHandler.SetActionsProvider(IMiniGameActionsProvider actionsProvider)
        {
            _actionsProvider = actionsProvider;
        }

        [Server]
        void IMiniGameEventsHandler.SendEvents(bool isCorrectAction, string guid)
        {
            Assert.IsFalse(_stakeholdersByIds.Count == 0);

            var stakeholders = _stakeholdersByIds[guid];

            if (isCorrectAction)
            {
                Debug.Log("CorrectAction");
            }
            else
            {
                Debug.Log("IncorrectAction");
                HandleMiniGameLost.Invoke(stakeholders.Player);
            }

            stakeholders.Observer.HandleMiniGameFinished();
        }

        [Server]
        void IMiniGameHandler.CallMiniGame(IMiniGameObserver observer, IPlayer player)
        {
            float correctPosition = Random.value;

            string guid = Guid.NewGuid().ToString();

            var data = new MiniGameData
            {
                Guid = guid,
                Duration = _settings.GameDurationInSeconds,
                CorrectPosition = correctPosition,
                MovementSpeed = _settings.MovementSpeed,
                CorrectAreaSize = _view.GetCorrectAreaSize()
            };

            var stakeholders = new StakeholdersData(observer, player);
            _stakeholdersByIds.Add(guid, stakeholders);

            player.InitializeMiniGame(player.GetId(), data);
        }

        void IUpdatable.Update()
        {
            if (_initialized)
            {
                _model.Move(Time.deltaTime);
                _view.SetFlagPosition(_model.Position);

                if (_model.IsTimeOver())
                {
                    _actionsProvider.CmdSendEvents(false, _model.Guid);

                    StopMiniGame();
                }
            }
        }

        [Client]
        private void HandleClick()
        {
            bool isCorrectClick = _model.CheckPosition();

            StopMiniGame();

            _actionsProvider.CmdSendEvents(isCorrectClick, _model.Guid);
        }

        [Client]
        private void StopMiniGame()
        {
            SetPauseState(false);
            _view.Disable();
            _initialized = false;
        }

        [Client]
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
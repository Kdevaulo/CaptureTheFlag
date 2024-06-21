using Kdevaulo.CaptureTheFlag.CaptureFlagBehaviour;
using Kdevaulo.CaptureTheFlag.MiniGameBehaviour;
using Kdevaulo.CaptureTheFlag.Networking;
using Kdevaulo.CaptureTheFlag.PlayerBehaviour;
using Kdevaulo.CaptureTheFlag.UIMessageBehaviour;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag
{
    [AddComponentMenu(nameof(EntryPoint) + " in " + nameof(CaptureTheFlag))]
    public class EntryPoint : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Joystick _joystick;
        [SerializeField] private UIMessageView _uiMessageView;
        [SerializeField] private UIMessageSettings _uiMessageSettings;
        [Header("MiniGame")]
        [SerializeField] private MiniGameView _miniGameView;
        [SerializeField] private MiniGameSettings _miniGameSettings;
        [Header("Flag")]
        [SerializeField] private FlagView _flagView;
        [SerializeField] private Transform _flagsContainer;
        [SerializeField] private FlagSettings _flagSettings;
        [Header("Player")]
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private Transform _playersContainer;
        [SerializeField] private PlayerSettings _playerSettings;
        [SerializeField] private ClientDataProvider _clientDataProvider;
        [Header("Network")]
        [SerializeField] private NetworkBehaviourHandler _networkHandler;

        private IClearable[] _clearables;
        private IClientDisconnectionHandler[] _disconnectionHandlers;

        private PlayerFactory _factory;
        private PlayerTuner _playerTuner;

        private FlagsController _flagsController;
        private FlagFactory _flagFactory;
        private MiniGameController _miniGameController;
        private PlayerController _playerController;

        private PlayerMover _playerMover;
        //private IReinitializable[] _reinitializables;
        private UIMessageController _uiMessageController;
        private IUpdatable[] _updatables;

        private UserInputHandler _userInputHandler;

        private void Awake()
        {
            SetupClasses();
            SetupInterfaces();
            SubscribeEvents();
        }

        private void Update()
        {
            foreach (var item in _updatables)
            {
                item.Update();
            }
        }

        private void SetupClasses()
        {
            _userInputHandler = new UserInputHandler(_joystick);

            _factory = new PlayerFactory(_playerView, _playersContainer);
            _flagFactory = new FlagFactory(_flagView, _flagSettings, _flagsContainer);

            _playerMover = new PlayerMover(_playerSettings);
            _playerTuner = new PlayerTuner(_playerSettings);

            _uiMessageController = new UIMessageController(_uiMessageView, _uiMessageSettings);
            _miniGameController = new MiniGameController(_miniGameView, _miniGameSettings, _userInputHandler);
            _flagsController = new FlagsController(_flagSettings, _flagFactory, _miniGameController, _playerTuner);
            _playerController = new PlayerController(_factory, _playerMover, _clientDataProvider, _userInputHandler);

            _flagSettings.Initialize();
            _clientDataProvider.Initialize(_miniGameController, _miniGameController);
            _networkHandler.Initialize(_playerController, _playerController, _playerTuner, _miniGameController);
        }

        private void SetupInterfaces()
        {
            _clearables = new IClearable[] { _flagsController };

            _updatables = new IUpdatable[]
                { _userInputHandler, _flagsController, _miniGameController, _uiMessageController };

            _disconnectionHandlers = new IClientDisconnectionHandler[] { _playerController, _flagsController};
        }

        private void SubscribeEvents()
        {
            _networkHandler.ClientDisconnected += ClearClient;
            _networkHandler.ClientDisconnectedFromServer += HandleClientDisconnected;
        }

        /// <summary>
        /// Works on Client
        /// </summary>
        private void ClearClient()
        {
            foreach (var clearable in _clearables)
            {
                clearable.Clear();
            }
        }

        /// <summary>
        /// Works on Server
        /// </summary>
        private void HandleClientDisconnected(int connectionId)
        {
            foreach (var handler in _disconnectionHandlers)
            {
                handler.HandleClientDisconnected(connectionId);
            }
        }
    }
}
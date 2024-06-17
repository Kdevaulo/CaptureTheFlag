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
        [Header("Network")]
        [SerializeField] private NetworkBehaviourHandler _networkHandler;

        private UserInputHandler _userInputHandler;
        
        private PlayerFactory _factory;
        private FlagSpawner _flagSpawner;
        
        private FlagsController _flagsController;
        private PlayerController _playerController;
        private MiniGameController _miniGameController;
        private UIMessageController _uiMessageController;

        private IUpdatable[] _updatables;
        private IReinitializable[] _reinitializables;

        public IColorGetter ColorGetter => _playerController;
        public IFlagSpawner FlagSpawner => _flagsController;
        public IFlagInvaderObserver InvaderObserver => _flagsController;

        private void Awake()
        {
            _userInputHandler = new UserInputHandler(_joystick);
            
            _factory = new PlayerFactory(_playerView, _playersContainer);
            _flagSpawner = new FlagSpawner(_flagView, _flagSettings, _flagsContainer);
            var playerMovement = new PlayerMovement(_userInputHandler, _playerSettings);

            _uiMessageController = new UIMessageController(_uiMessageView, _uiMessageSettings);
            _miniGameController = new MiniGameController(_miniGameView, _miniGameSettings, _userInputHandler);
            _flagsController = new FlagsController(_flagSettings, _flagSpawner, _miniGameController);
            _playerController =
                new PlayerController(_networkHandler,  playerMovement, _factory, _playerSettings);

            _flagSettings.Initialize();
            _networkHandler.SetMessageCaller(_miniGameController);

            _updatables = new IUpdatable[] { _userInputHandler, _flagsController, _miniGameController, _uiMessageController };
            _reinitializables = new IReinitializable[] { _flagSettings, _flagSpawner, _flagsController };
        }

        private void Update()
        {
            foreach (var item in _updatables)
            {
                item.Update();
            }
        }

        private void Reinitialize()
        {
            foreach (var item in _reinitializables)
            {
                item.Reinitialize();
            }
        }

        public void HandleClientDisconnected()
        {
            Reinitialize();
        }
    }
}
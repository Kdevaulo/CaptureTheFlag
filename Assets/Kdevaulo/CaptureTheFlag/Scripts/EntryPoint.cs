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
        [Header("Common")]
        [SerializeField] private UserInput _userInput;
        [Header("UI")]
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

        private PlayerFactory _factory;
        private FlagsController _flagsController;
        private FlagSpawner _flagSpawner;
        private MiniGameController _miniGameController;
        private PlayerController _playerController;

        private IReinitializable[] _reinitializables;
        private UIMessageController _uiMessageController;
        private IUpdatable[] _updatables;

        public IColorGetter ColorGetter => _playerController;
        public IFlagSpawner FlagSpawner => _flagsController;
        public IFlagInvaderObserver InvaderObserver => _flagsController;

        private void Awake()
        {
            _factory = new PlayerFactory(_playerView, _playersContainer);
            _flagSpawner = new FlagSpawner(_flagView, _flagSettings, _flagsContainer);
            _flagSettings.Initialize();
            //var playerMovement = new PlayerMovement(_userInput, _playerSettings);

            _uiMessageController = new UIMessageController(_uiMessageView, _uiMessageSettings);
            _miniGameController = new MiniGameController(_miniGameView, _miniGameSettings);
            _flagsController = new FlagsController(_flagSettings, _flagSpawner, _miniGameController);
            _playerController =
                new PlayerController(_networkHandler, /* playerMovement,*/ _factory, _playerSettings);

            _networkHandler.SetMessageCaller(_miniGameController);

            _updatables = new IUpdatable[] { _userInput, _flagsController, _miniGameController, _uiMessageController };
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
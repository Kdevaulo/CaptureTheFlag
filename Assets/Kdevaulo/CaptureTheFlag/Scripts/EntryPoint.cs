using Kdevaulo.CaptureTheFlag.CaptureFlagBehaviour;
using Kdevaulo.CaptureTheFlag.Networking;
using Kdevaulo.CaptureTheFlag.PlayerBehaviour;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag
{
    [AddComponentMenu(nameof(EntryPoint) + " in " + nameof(CaptureTheFlag))]
    public class EntryPoint : MonoBehaviour
    {
        [Header("Common")]
        [SerializeField] private UserInput _userInput;
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
        private PlayerController _playerController;
        private IReinitializable[] _reinitializables;

        private IUpdatable[] _updatables;

        public IColorGetter ColorGetter => _playerController;
        public IFlagSpawner FlagSpawner => _flagsController;
        public IFlagInvaderObserver InvaderObserver => _flagsController;

        private void Awake()
        {
            //_factory = new PlayerFactory(_playerView, _playersContainer);
            _flagSpawner = new FlagSpawner(_flagView, _flagSettings, _flagsContainer);
            _flagSettings.Initialize();
            //var playerMovement = new PlayerMovement(_userInput, _playerSettings);
            _flagsController = new FlagsController(_flagSettings, _flagSpawner);
            _playerController =
                new PlayerController(_networkHandler, /* playerMovement,*/ /*_factory,*/_playerSettings);

            _updatables = new IUpdatable[] { _userInput, _flagsController };
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
            Debug.Log("Reinitialize");

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
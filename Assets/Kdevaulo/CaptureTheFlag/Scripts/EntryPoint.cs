using Kdevaulo.CaptureTheFlag.CaptureFlagBehaviour;
using Kdevaulo.CaptureTheFlag.Networking;
using Kdevaulo.CaptureTheFlag.PlayerBehaviour;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag
{
    [AddComponentMenu(nameof(EntryPoint) + " in " + nameof(CaptureTheFlag))]
    public class EntryPoint : MonoBehaviour
    {
        public IColorGetter ColorGetter => _playerSettings;
        public FlagSpawner FlagSpawner => _flagSpawner;
        
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
        private FlagSpawner _flagSpawner;

        private IUpdatable[] _updatables;

        private void Awake()
        {
            //_factory = new PlayerFactory(_playerView, _playersContainer);
            _flagSpawner = new FlagSpawner(_flagView, _flagSettings,_flagsContainer);
            _flagSettings.Initialize();
            //var playerMovement = new PlayerMovement(_userInput, _playerSettings);
            var playerHandler = new PlayerHandler(_networkHandler, /* playerMovement,*/ /*_factory,*/_playerSettings);

            _updatables = new IUpdatable[] { _userInput };
        }

        private void Update()
        {
            foreach (var updatable in _updatables)
            {
                updatable.Update();
            }
        }
    }
}
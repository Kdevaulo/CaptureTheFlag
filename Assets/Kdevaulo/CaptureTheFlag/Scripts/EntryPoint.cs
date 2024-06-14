using Kdevaulo.CaptureTheFlag.Networking;
using Kdevaulo.CaptureTheFlag.PlayerBehaviour;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag
{
    [AddComponentMenu(nameof(EntryPoint) + " in " + nameof(CaptureTheFlag))]
    public class EntryPoint : MonoBehaviour
    {
        public IColorGetter ColorGetter => _playerSettings;
        
        [Header("Common")]
        [SerializeField] private UserInput _userInput;
        [Header("Player")]
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private Transform _playersContainer;
        [SerializeField] private PlayerSettings _playerSettings;
        [Header("Network")]
        [SerializeField] private NetworkBehaviourHandler _networkHandler;

        private PlayerFactory _factory;

        private IUpdatable[] _updatables;

        private void Awake()
        {
            //_factory = new PlayerFactory(_playerView, _playersContainer);

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
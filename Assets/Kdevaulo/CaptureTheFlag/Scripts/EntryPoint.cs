using System;

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
        [Header("Player")]
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private Transform _playersContainer;
        [SerializeField] private PlayerSettings _playerSettings;
        [Header("Network")]
        [SerializeField] private NetworkBehaviourHandler _networkHandler;

        private PlayerFactory _factory;
        private PlayerPool _pool;

        private IUpdatable[] _updatables;

        private void Awake()
        {
            _factory = new PlayerFactory(_playerView, _playersContainer);
            _pool = new PlayerPool(_factory, _playerSettings);

            var playerMovement = new PlayerMovement(_userInput, _playerSettings);
            var playerHandler = new PlayerHandler(_networkHandler, playerMovement, _pool);

            _updatables = new IUpdatable[] { _userInput };
        }

        private void Start()
        {
            _pool.Initialize();
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
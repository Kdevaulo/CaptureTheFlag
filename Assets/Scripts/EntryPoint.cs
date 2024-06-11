using System;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag
{
    [AddComponentMenu(nameof(EntryPoint) + " in " + nameof(CaptureTheFlag))]
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private Transform _playersContainer;
        [SerializeField] private PlayerSettings _playerSettings;

        private PlayerFactory _factory;
        private PlayerPool _pool;

        private void Awake()
        {
            _factory = new PlayerFactory(_playerView, _playersContainer);
            _pool = new PlayerPool(_factory, _playerSettings);
        }

        private void Start()
        {
            _pool.Initialize();
        }
    }
}
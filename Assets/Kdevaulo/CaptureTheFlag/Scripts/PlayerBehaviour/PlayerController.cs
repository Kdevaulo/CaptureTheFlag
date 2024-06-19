using System.Collections.Generic;

using Mirror;

using UnityEngine;
using UnityEngine.Assertions;

namespace Kdevaulo.CaptureTheFlag.PlayerBehaviour
{
    public class PlayerController : IPlayerProvider, IPlayerMovementHandler, IClientDisconnectionHandler
    {
        private readonly IMovementProvider _movementProvider;

        private readonly PlayerMover _mover;
        private readonly PlayerFactory _factory;
        private readonly MovableProvider _movableProvider;

        private Dictionary<int, PlayerView> _playersByIds = new Dictionary<int, PlayerView>();

        private IMovable _localPlayer;

        public PlayerController(PlayerFactory factory, PlayerMover mover, MovableProvider movableProvider,
            IMovementProvider movementProvider)
        {
            _factory = factory;
            _mover = mover;
            _movableProvider = movableProvider;

            _movementProvider = movementProvider;

            _movableProvider.MovableSet += HandleMovableSet;

            _movementProvider.MoveHorizontal += HandleHorizontalMovement;
            _movementProvider.MoveVertical += HandleVerticalMovement;
        }

        [Client]
        private void HandleMovableSet()
        {
            _localPlayer = _movableProvider.Movable;
        }

        [Client]
        private void HandleHorizontalMovement(float offset)
        {
            _localPlayer.TryMove(offset, 0);
        }

        [Client]
        private void HandleVerticalMovement(float offset)
        {
            _localPlayer.TryMove(0, offset);
        }

        [Server]
        void IPlayerMovementHandler.TryMovePlayer(int id, float moveHorizontal, float moveVertical)
        {
            Assert.IsTrue(_playersByIds.ContainsKey(id));

            var player = _playersByIds[id];

            _mover.HandlePlayerMovement(player, moveHorizontal, moveVertical);
        }

        [Server]
        PlayerView IPlayerProvider.SpawnPlayer(int id)
        {
            var view = _factory.Create();

            _playersByIds.Add(id, view);

            return view;
        }

        [Server]
        void IClientDisconnectionHandler.HandleClientDisconnected(int id)
        {
            Assert.IsTrue(_playersByIds.ContainsKey(id));
            
            _playersByIds.Remove(id);
        }
    }
}
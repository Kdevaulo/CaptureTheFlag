﻿using System.Collections.Generic;

using Mirror;

using UnityEngine.Assertions;

namespace Kdevaulo.CaptureTheFlag.PlayerBehaviour
{
    public class PlayerController : IPlayerProvider, IPlayerMovementHandler, IClientDisconnectionHandler
    {
        private readonly IMovementProvider _movementProvider;

        private readonly PlayerMover _mover;
        private readonly PlayerFactory _factory;
        private readonly ClientDataProvider _movableProvider;

        private IMovable _localPlayer;

        private Dictionary<int, PlayerView> _playersByIds = new Dictionary<int, PlayerView>();

        public PlayerController(PlayerFactory factory, PlayerMover mover, ClientDataProvider movableProvider,
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

        [Server]
        void IClientDisconnectionHandler.HandleClientDisconnected(int id)
        {
            Assert.IsTrue(_playersByIds.ContainsKey(id));

            _playersByIds.Remove(id);
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

        [ClientCallback]
        private void HandleHorizontalMovement(float offset)
        {
            _localPlayer.TryMove(offset, 0);
        }

        [Client]
        private void HandleMovableSet()
        {
            _localPlayer = _movableProvider.Movable;
        }

        [Client]
        private void HandleVerticalMovement(float offset)
        {
            _localPlayer.TryMove(0, offset);
        }
    }
}
using Mirror;

using UnityEngine;
using UnityEngine.Assertions;

namespace Kdevaulo.CaptureTheFlag.PlayerBehaviour
{
    [AddComponentMenu(nameof(PlayerView) + " in " + nameof(PlayerBehaviour))]
    public class PlayerView : NetworkBehaviour, IPlayer, IMovable
    {
        [SerializeField] private MeshRenderer _meshRenderer;

        private IPlayerMovementHandler _movementHandler;

        [SyncVar(hook = nameof(HandleColorUpdated))]
        private Color _color;

        [SyncVar(hook = nameof(HandlePositionChanged))]
        private Vector3 _position;

        private int _connectionId = -1;

        private MaterialPropertyBlock _propertyBlock;

        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
        }

        [Client]
        Vector3 IPlayer.GetPosition()
        {
            return transform.position;
        }

        [Client]
        GameObject IPlayer.GetOwner()
        {
            return gameObject;
        }

        [Client]
        void IMovable.TryMove(float moveHorizontal, float moveVertical)
        {
            Assert.IsTrue(isLocalPlayer);

            MovePlayer(_connectionId, moveHorizontal, moveVertical);
        }

        [Server]
        public void Initialize(NetworkConnectionToClient connection, IPlayerMovementHandler movementHandler)
        {
            _movementHandler = movementHandler;
            HandlePlayerInitialized(connection, connection.connectionId);
        }

        [TargetRpc]
        private void HandlePlayerInitialized(NetworkConnectionToClient _, int connectionId)
        {
            var mediator = FindObjectOfType<MovableProvider>();
            mediator.SetMovable(this);

            _connectionId = connectionId;

            Debug.Log("On Player Initialized");
        }

        [Server]
        public void HandleMovement(float moveHorizontal, float moveVertical)
        {
            var movement = new Vector3(moveHorizontal, 0, moveVertical);
            _position += movement;
        }

        [Server]
        public void SetPosition(Vector3 position)
        {
            _position = position;
        }

        [Server]
        public void SetColor(Color color)
        {
            _color = color;
        }

        [Command]
        private void MovePlayer(int connectionId, float moveHorizontal, float moveVertical)
        {
            _movementHandler.TryMovePlayer(connectionId, moveHorizontal, moveVertical);
        }

        [Client]
        private void HandleColorUpdated(Color _, Color newColor)
        {
            _propertyBlock.SetColor("_Color", newColor);
            _meshRenderer.SetPropertyBlock(_propertyBlock);
        }

        [Client]
        private void HandlePositionChanged(Vector3 _, Vector3 newPosition)
        {
            transform.position = newPosition;
        }
    }
}
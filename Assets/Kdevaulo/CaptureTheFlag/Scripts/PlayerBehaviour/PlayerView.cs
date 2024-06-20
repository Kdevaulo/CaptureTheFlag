using Kdevaulo.CaptureTheFlag.MiniGameBehaviour;

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

        private MonoBehaviourProvider _mediator;

        private MaterialPropertyBlock _propertyBlock;

        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
        }

        int IPlayer.GetId()
        {
            return _connectionId;
        }

        [Client]
        Vector3 IPlayer.GetPosition()
        {
            return transform.position;
        }

        [Server]
        void IPlayer.InitializeMiniGame(int connectionId, MiniGameData data)
        {
            var targetConnection = NetworkServer.connections[connectionId];
            InitializeMiniGame(targetConnection, data);
        }

        [TargetRpc]
        private void InitializeMiniGame(NetworkConnectionToClient _, MiniGameData data)
        {
            _mediator.InitializeMiniGame(data);
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

            Assert.IsFalse(_connectionId == -1, "ConnectionId == -1");

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
            _mediator = FindObjectOfType<MonoBehaviourProvider>();
            _mediator.SetMovable(this);

            Assert.IsFalse(connectionId == -1, "ConnectionId == -1");

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
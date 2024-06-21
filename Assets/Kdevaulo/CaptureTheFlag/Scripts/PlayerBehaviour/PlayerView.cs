using Kdevaulo.CaptureTheFlag.MiniGameBehaviour;

using Mirror;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.PlayerBehaviour
{
    [AddComponentMenu(nameof(PlayerView) + " in " + nameof(PlayerBehaviour))]
    public class PlayerView : NetworkBehaviour, IPlayer, IMovable
    {
        [SerializeField] private MeshRenderer _meshRenderer;

        private IMiniGameEventsHandler _miniGameEventsHandler;
        private IPlayerMovementHandler _movementHandler;

        [SyncVar(hook = nameof(HandleColorUpdated))]
        private Color _color;

        [SyncVar(hook = nameof(HandlePositionChanged))]
        private Vector3 _position;

        private ClientDataProvider _mediator;
        private MaterialPropertyBlock _propertyBlock;

        private int _connectionId = -1;

        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
        }

        [Client]
        void IMovable.TryMove(float moveHorizontal, float moveVertical)
        {
            CmdMovePlayer(_connectionId, moveHorizontal, moveVertical);
        }

        [Command(requiresAuthority = false)]
        void IMiniGameActionsProvider.CmdSendEvents(bool isCorrectAction, string guid)
        {
            _miniGameEventsHandler.SendEvents(isCorrectAction, guid);
        }

        [Server]
        void IMiniGameActionsProvider.InitializeMiniGame(int connectionId, MiniGameData data)
        {
            var targetConnection = NetworkServer.connections[connectionId];
            InitializeMiniGame(targetConnection, data);
        }

        int IPlayer.GetId()
        {
            return _connectionId;
        }

        [Server]
        Vector3 IPlayer.GetPosition()
        {
            return _position;
        }

        [Server]
        GameObject IPlayer.GetOwner()
        {
            return gameObject;
        }

        [Server]
        public void Initialize(NetworkConnectionToClient connection, IPlayerMovementHandler movementHandler,
            IMiniGameEventsHandler eventsHandler)
        {
            _miniGameEventsHandler = eventsHandler;
            _miniGameEventsHandler.SetActionsProvider(this);

            _movementHandler = movementHandler;

            _connectionId = connection.connectionId;

            HandlePlayerInitialized(connection, _connectionId);
        }

        [Server]
        public void HandleMovement(float moveHorizontal, float moveVertical)
        {
            var movement = new Vector3(moveHorizontal, 0, moveVertical);
            _position += movement;

            transform.position = _position;
        }

        [Server]
        public void SetPosition(Vector3 position)
        {
            _position = position;
            transform.position = _position;
        }

        [Server]
        public void SetColor(Color color)
        {
            _color = color;
            ChangeColor(_color);
        }

        [Command]
        private void CmdMovePlayer(int connectionId, float moveHorizontal, float moveVertical)
        {
            _movementHandler.TryMovePlayer(connectionId, moveHorizontal, moveVertical);
        }

        [TargetRpc]
        private void HandlePlayerInitialized(NetworkConnectionToClient _, int connectionId)
        {
            _mediator = FindObjectOfType<ClientDataProvider>();
            _mediator.SetMovable(this);

            var eventsHandler = _mediator.GetEventsHandler();
            eventsHandler.SetActionsProvider(this);

            _connectionId = connectionId;
        }

        [TargetRpc]
        private void InitializeMiniGame(NetworkConnectionToClient _, MiniGameData data)
        {
            _mediator.InitializeMiniGame(data);
        }

        [Client]
        private void HandleColorUpdated(Color _, Color newColor)
        {
            ChangeColor(newColor);
        }

        /// <summary>
        /// Works on Server and Client
        /// </summary>
        private void ChangeColor(Color newColor)
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
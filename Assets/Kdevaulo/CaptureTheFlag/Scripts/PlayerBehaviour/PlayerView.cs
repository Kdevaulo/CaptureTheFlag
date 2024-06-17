using Mirror;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.PlayerBehaviour
{
    [AddComponentMenu(nameof(PlayerView) + " in " + nameof(PlayerBehaviour))]
    public class PlayerView : NetworkBehaviour, IFlagInvader
    {
        [SerializeField] private MeshRenderer _mesh;

        [SyncVar(hook = nameof(HandleColorUpdated))]
        private Color _color;

        [SyncVar(hook = nameof(HandlePositionChanged))]
        private Vector3 _localPosition;

        private MaterialPropertyBlock _propertyBlock;

        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
        }

        [Client]
        Vector3 IFlagInvader.GetPosition()
        {
            return transform.position;
        }

        [Client]
        public NetworkIdentity GetNetIdentity()
        {
            return netIdentity;
        }

        [Server]
        public void SetColor(Color color)
        {
            _color = color;
        }

        [Server]
        public void SetPosition(Vector3 offset)
        {
            _localPosition += offset;
        }

        [Server]
        public void SetDefaultPosition()
        {
            _localPosition = Vector3.zero;
        }

        [Client]
        private void HandleColorUpdated(Color oldColor, Color newColor)
        {
            _propertyBlock.SetColor("_Color", newColor);
            _mesh.SetPropertyBlock(_propertyBlock);
        }

        [Client]
        private void HandlePositionChanged(Vector3 oldPosition, Vector3 newPosition)
        {
            transform.localPosition = newPosition;
        }
    }
}
using System;

using Mirror;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.CaptureFlagBehaviour
{
    [AddComponentMenu(nameof(FlagView) + " in " + nameof(CaptureFlagBehaviour))]
    public class FlagView : NetworkBehaviour
    {
        [SerializeField] private MeshRenderer _mesh;

        private MaterialPropertyBlock _propertyBlock;

        [SyncVar(hook = nameof(HandleColorChanged))]
        private Color _color;

        [SyncVar(hook = nameof(HandlePositionChanged))]
        private Vector3 _position;

        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
        }

        [Server]
        public void SetPosition(Vector3 targetPosition)
        {
            _position = targetPosition;
        }

        [Server]
        public void SetColor(Color color)
        {
            _color = color;
        }

        private void HandleColorChanged(Color _, Color newColor)
        {
            _propertyBlock.SetColor("_Color", newColor);
            _mesh.SetPropertyBlock(_propertyBlock);
        }

        private void HandlePositionChanged(Vector3 _, Vector3 newPosition)
        {
            transform.position = newPosition;
        }
    }
}
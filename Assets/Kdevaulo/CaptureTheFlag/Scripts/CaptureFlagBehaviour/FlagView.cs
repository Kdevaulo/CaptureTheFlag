using Mirror;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.CaptureFlagBehaviour
{
    [AddComponentMenu(nameof(FlagView) + " in " + nameof(CaptureFlagBehaviour))]
    public class FlagView : NetworkBehaviour
    {
        [SerializeField] private MeshRenderer _mesh;

        private MaterialPropertyBlock _propertyBlock;

        public void SetPosition(Vector3 targetPosition)
        {
            transform.localPosition = targetPosition;
        }

        public void SetColor(Color color)
        {
            _propertyBlock ??= new MaterialPropertyBlock();
            _propertyBlock.SetColor("_Color", color);
            _mesh.SetPropertyBlock(_propertyBlock);
        }
    }
}
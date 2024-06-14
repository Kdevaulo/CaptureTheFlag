using Mirror;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.PlayerBehaviour
{
    [AddComponentMenu(nameof(PlayerView) + " in " + nameof(PlayerBehaviour))]
    public class PlayerView : NetworkBehaviour
    {
        [SerializeField] private MeshRenderer _mesh;

        private MaterialPropertyBlock _propertyBlock;

        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();

            var entryPoint = FindObjectOfType<EntryPoint>();
            var colorGetter = entryPoint.ColorGetter;
            SetColor(colorGetter.GetColor());
        }

        public void SetColor(Color color)
        {
            _propertyBlock.SetColor("_Color", color);
            _mesh.SetPropertyBlock(_propertyBlock);
        }

        public void Move(Vector3 offset)
        {
            if (isLocalPlayer)
            {
                transform.localPosition += offset;
            }
        }

        public void SetDefaultPosition()
        {
            transform.localPosition = Vector3.zero;
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
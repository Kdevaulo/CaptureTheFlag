using System;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag
{
    [AddComponentMenu(nameof(PlayerView) + " in " + nameof(CaptureTheFlag))]
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _mesh;

        private MaterialPropertyBlock _propertyBlock;

        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
        }

        public void SetColor(Color color)
        {
            _propertyBlock.SetColor("_Color", color);
            _mesh.SetPropertyBlock(_propertyBlock);
        }

        public void Move(Vector3 offset)
        {
            transform.localPosition += offset;
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
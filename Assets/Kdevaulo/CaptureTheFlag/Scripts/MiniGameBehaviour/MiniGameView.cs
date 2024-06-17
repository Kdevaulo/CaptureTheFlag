using System;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Kdevaulo.CaptureTheFlag.MiniGameBehaviour
{
    [AddComponentMenu(nameof(MiniGameView) + " in " + nameof(MiniGameBehaviour))]
    public class MiniGameView : MonoBehaviour, IPointerClickHandler
    {
        public event Action<MiniGameView> Clicked = delegate { };

        [SerializeField] private Scrollbar _correctZoneScrollbar;

        [SerializeField] private GameObject _miniGameContainer;

        [SerializeField] private RectTransform _flagContainer;
        [SerializeField] private RectTransform _flagTransform;

        [Min(0)]
        [SerializeField] private float _boundsOffset;

        private float _maxPosition;

        private bool _movingRight;

        private void Awake()
        {
            _maxPosition = _flagContainer.rect.width - _boundsOffset;
            _flagTransform.anchoredPosition = new Vector2(_boundsOffset, 0);
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            Clicked.Invoke(this);
        }

        public float GetCorrectAreaSize()
        {
            return _correctZoneScrollbar.size;
        }

        public void SetCorrectAreaPosition(float percentage)
        {
            _correctZoneScrollbar.value = Mathf.Clamp01(percentage);
        }

        public void SetFlagPosition(float percentage)
        {
            _flagTransform.anchoredPosition = new Vector2(percentage * _maxPosition, 0);
        }

        public void Enable()
        {
            _miniGameContainer.SetActive(true);
        }

        public void Disable()
        {
            _miniGameContainer.SetActive(false);
        }
    }
}
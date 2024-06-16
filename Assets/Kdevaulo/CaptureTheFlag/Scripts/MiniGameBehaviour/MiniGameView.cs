using UnityEngine;
using UnityEngine.UI;

namespace Kdevaulo.CaptureTheFlag.MiniGameBehaviour
{
    [AddComponentMenu(nameof(MiniGameView) + " in " + nameof(MiniGameBehaviour))]
    public class MiniGameView : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private Scrollbar _correctZoneScrollbar;
        [SerializeField] private RectTransform _flagTransform;

        [Min(0)]
        [SerializeField] private float _boundsOffset;

        private float _maxPosition;

        private bool _movingRight;

        private void Awake()
        {
            var scrollTransform = _correctZoneScrollbar.GetComponent<RectTransform>();
            _maxPosition = scrollTransform.rect.width - _boundsOffset;
            _flagTransform.anchoredPosition = new Vector2(_boundsOffset, 0);
        }

        public void SetCorrectAreaPosition(float percentage)
        {
            _correctZoneScrollbar.value = Mathf.Clamp01(percentage);
        }

        public void MoveFlag(float speed)
        {
            if (_flagTransform.anchoredPosition.x + speed > _maxPosition ||
                _flagTransform.anchoredPosition.x - speed < _boundsOffset)
            {
                _movingRight = !_movingRight;
            }

            var offset = _movingRight ? new Vector2(speed, 0) : new Vector2(-speed, 0);

            _flagTransform.anchoredPosition += offset;
        }

        public void Enable()
        {
            _container.gameObject.SetActive(true);
        }

        public void Disable()
        {
            _container.gameObject.SetActive(false);
        }
    }
}
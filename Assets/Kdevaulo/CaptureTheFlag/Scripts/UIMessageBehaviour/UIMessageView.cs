using UnityEngine;
using UnityEngine.UI;

namespace Kdevaulo.CaptureTheFlag.UIMessageBehaviour
{
    [AddComponentMenu(nameof(UIMessageView) + " in " + nameof(UIMessageBehaviour))]
    public class UIMessageView : MonoBehaviour
    {
        [SerializeField] private Text _textHolder;

        public void ActivateMessage(string message)
        {
            _textHolder.text = message;
            _textHolder.enabled = true;
        }

        public void DisableText()
        {
            _textHolder.text = string.Empty;
            _textHolder.enabled = false;
        }
    }
}
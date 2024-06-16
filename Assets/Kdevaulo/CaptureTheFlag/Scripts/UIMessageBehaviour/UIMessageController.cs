using Kdevaulo.CaptureTheFlag.Networking;

using Mirror;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.UIMessageBehaviour
{
    public class UIMessageController : IUpdatable
    {
        private readonly UIMessageView _messageView;
        private readonly UIMessageSettings _settings;

        private bool _isActive;

        private float _secondsToDisable;

        public UIMessageController(UIMessageView messageView, UIMessageSettings settings)
        {
            _messageView = messageView;
            _settings = settings;

            _messageView.DisableText();

            NetworkClient.RegisterHandler<MiniGameLoseMessage>(HandleMessage);
        }

        void IUpdatable.Update()
        {
            if (_isActive)
            {
                _secondsToDisable -= Time.deltaTime;

                if (_secondsToDisable <= 0)
                {
                    _messageView.DisableText();
                    _isActive = false;
                }
            }
        }

        private void ActivateMessage(string message)
        {
            _messageView.ActivateMessage(message);

            _secondsToDisable = _settings.MessageDurationInSeconds;
            _isActive = true;
        }

        private void HandleMessage(MiniGameLoseMessage message)
        {
            ActivateMessage(message.Message);
        }
    }
}
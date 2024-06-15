using Mirror;

using UnityEngine;
using UnityEngine.UI;

namespace Kdevaulo.CaptureTheFlag
{
    [AddComponentMenu(nameof(CustomHUD) + " in " + nameof(CaptureTheFlag))]
    public class CustomHUD : MonoBehaviour
    {
        [SerializeField] private Button _startHostButton;
        [SerializeField] private Button _stopHostButton;
        [Space]
        [SerializeField] private Button _startClientButton;
        [SerializeField] private Button _stopClientButton;
        [Space]
        [SerializeField] private Button _startServerOnlyButton;
        [SerializeField] private Button _stopServerButton;
        [Space]
        [SerializeField] private NetworkManager _networkManager;

        private bool _hostStarted;
        private bool _serverStarted;

        private void Awake()
        {
            _startHostButton.onClick.AddListener(StartHost);
            _stopHostButton.onClick.AddListener(StopHost);

            _startClientButton.onClick.AddListener(StartClient);
            _stopClientButton.onClick.AddListener(StopClient);

            _startServerOnlyButton.onClick.AddListener(StartServer);
            _stopServerButton.onClick.AddListener(StopServer);
        }

        private void StopServer()
        {
            _networkManager.StopServer();

            _serverStarted = false;

            Enable(_startHostButton);
            Disable(_stopHostButton);

            Enable(_startClientButton);
            Disable(_stopClientButton);

            Enable(_startServerOnlyButton);
            Disable(_stopServerButton);
        }

        private void StartServer()
        {
            _networkManager.StartServer();

            _serverStarted = true;

            Disable(_startHostButton);
            Disable(_stopHostButton);

            Enable(_startClientButton);
            Disable(_stopClientButton);

            Disable(_startServerOnlyButton);
            Enable(_stopServerButton);
        }

        private void StopClient()
        {
            _networkManager.StopClient();

            bool clientOnly = !_hostStarted && !_serverStarted;

            SetActiveState(clientOnly, _startHostButton, _startServerOnlyButton);

            SetActiveState(_hostStarted, _stopHostButton);
            SetActiveState(_serverStarted, _stopServerButton);

            Enable(_startClientButton);
            Disable(_stopClientButton);
        }

        private void StartClient()
        {
            _networkManager.StartClient();

            Disable(_startHostButton);
            SetActiveState(_hostStarted, _stopHostButton);

            Disable(_startClientButton);
            Enable(_stopClientButton);

            Disable(_startServerOnlyButton);
            SetActiveState(_serverStarted, _stopServerButton);
        }

        private void StopHost()
        {
            _networkManager.StopHost();

            _hostStarted = false;

            Enable(_startHostButton);
            Disable(_stopHostButton);

            Enable(_startClientButton);
            Disable(_stopClientButton);

            Enable(_startServerOnlyButton);
            Disable(_stopServerButton);
        }

        private void StartHost()
        {
            _networkManager.StartHost();

            _hostStarted = true;

            Disable(_startHostButton);
            Enable(_stopHostButton);

            Disable(_startClientButton);
            Enable(_stopClientButton);

            Disable(_startServerOnlyButton);
            Disable(_stopServerButton);
        }

        private void Enable(Button button)
        {
            button.gameObject.SetActive(true);
        }

        private void Disable(Button button)
        {
            button.gameObject.SetActive(false);
        }

        private void SetActiveState(bool state, params Button[] buttons)
        {
            foreach (var button in buttons)
            {
                button.gameObject.SetActive(state);
            }
        }
    }
}
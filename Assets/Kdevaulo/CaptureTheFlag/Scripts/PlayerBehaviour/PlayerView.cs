using Mirror;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.PlayerBehaviour
{
    [AddComponentMenu(nameof(PlayerView) + " in " + nameof(PlayerBehaviour))]
    public class PlayerView : NetworkBehaviour, IFlagInvader
    {
        [SerializeField] private MeshRenderer _mesh;

        private IFlagSpawner _flagSpawner;

        private MaterialPropertyBlock _propertyBlock;

        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();

            var entryPoint = FindObjectOfType<EntryPoint>();
            var colorGetter = entryPoint.ColorGetter;

            var color = colorGetter.GetColor();
            SetColor(color);

            _flagSpawner = entryPoint.FlagSpawner;
            _flagSpawner.Spawn(color, netIdentity);

            entryPoint.InvaderObserver.AddInvaders(this);

            NetworkClient.OnDisconnectedEvent += entryPoint.HandleClientDisconnected;
        }

        private void OnDestroy()
        {
            _flagSpawner.Clear(netIdentity);
        }

        Vector3 IFlagInvader.GetPosition()
        {
            return transform.position;
        }

        void IFlagInvader.HandleFlagCaptured()
        {
            //todo: capturedParticles
        }

        void IFlagInvader.HandleAllCaptured()
        {
            Debug.Log(_propertyBlock.GetColor("_Color") + " Won!");
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
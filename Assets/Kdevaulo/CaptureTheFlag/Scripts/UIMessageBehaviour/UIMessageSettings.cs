using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.UIMessageBehaviour
{
    [CreateAssetMenu(menuName = nameof(UIMessageBehaviour) + "/" + nameof(UIMessageSettings),
        fileName = nameof(UIMessageSettings))]
    public class UIMessageSettings : ScriptableObject
    {
        [field: SerializeField] public float MessageDurationInSeconds { get; private set; }
    }
}
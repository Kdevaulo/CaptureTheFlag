using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.MiniGameBehaviour
{
    [CreateAssetMenu(menuName = nameof(MiniGameBehaviour) + "/" + nameof(MiniGameSettings),
        fileName = nameof(MiniGameSettings))]
    public class MiniGameSettings : ScriptableObject
    {
        [field: Min(0)]
        [field: SerializeField] public float MovementSpeed { get; private set; }
        [field: Min(0)]
        [field: SerializeField] public float GameDurationInSeconds { get; private set; }
    }
}
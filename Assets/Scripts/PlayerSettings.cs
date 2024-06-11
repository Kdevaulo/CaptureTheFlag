using UnityEngine;
using UnityEngine.Assertions;

namespace Kdevaulo.CaptureTheFlag
{
    [CreateAssetMenu(menuName = nameof(CaptureTheFlag) + "/" + nameof(PlayerSettings),
        fileName = nameof(PlayerSettings))]
    public class PlayerSettings : ScriptableObject
    {
        [field: SerializeField] public Color[] SkinColors { get; private set; }
        [field: SerializeField] public int InitialPlayersCount { get; private set; }

        private int _currentColorIndex;

        public Color GetColor()
        {
            Assert.IsTrue(SkinColors.Length > 0);

            if (SkinColors.Length <= _currentColorIndex)
            {
                _currentColorIndex = 0;
            }

            return SkinColors[_currentColorIndex++];
        }
    }
}
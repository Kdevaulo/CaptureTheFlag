using System;

using UnityEngine;
using UnityEngine.Assertions;

namespace Kdevaulo.CaptureTheFlag.PlayerBehaviour
{
    [CreateAssetMenu(menuName = nameof(PlayerBehaviour) + "/" + nameof(PlayerSettings),
        fileName = nameof(PlayerSettings))]
    public class PlayerSettings : ScriptableObject
    {
        [field: SerializeField] public Color[] SkinColors { get; private set; }
        [field: Min(0)]
        [field: SerializeField] public float PlayerMovementSensitivity { get; private set; }

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
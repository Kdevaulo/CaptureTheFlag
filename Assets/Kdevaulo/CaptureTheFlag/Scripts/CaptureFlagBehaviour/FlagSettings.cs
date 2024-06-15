using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;

namespace Kdevaulo.CaptureTheFlag.CaptureFlagBehaviour
{
    [CreateAssetMenu(menuName = nameof(PlayerBehaviour) + "/" + nameof(FlagSettings),
        fileName = nameof(FlagSettings))]
    public class FlagSettings : ScriptableObject
    {
        [Min(0)]
        [field: SerializeField] public int FlagsCount;

        [Min(0)]
        [field: SerializeField] public float CursorSpeed;
        [Min(0)]
        [field: SerializeField] public float FlagRadiusInUnits;
        [Min(0)]
        [field: SerializeField] public float OnLoseDelay;

        [field: SerializeField] public Vector3[] SpawnPositions;

        private Dictionary<Vector3, bool> _flagsPositions;

        public void Initialize()
        {
            _flagsPositions = new Dictionary<Vector3, bool>(SpawnPositions.Length);
            Debug.Log(1);

            foreach (var position in SpawnPositions)
            {
                _flagsPositions.Add(position, false);
            }
        }

        public Vector3 GetPosition()
        {
            var targetPosition = BorrowFirstFreePosition();

            if (targetPosition == Vector3.positiveInfinity)
            {
                RefreshFlags();

                targetPosition = BorrowFirstFreePosition();

                Assert.IsFalse(targetPosition == Vector3.positiveInfinity);
            }

            return targetPosition;
        }

        private Vector3 BorrowFirstFreePosition()
        {
            foreach (var flagPosition in _flagsPositions)
            {
                if (flagPosition.Value == false)
                {
                    _flagsPositions[flagPosition.Key] = true;
                    return flagPosition.Key;
                }
            }

            return Vector3.positiveInfinity;
        }

        private void RefreshFlags()
        {
            foreach (var flagPosition in _flagsPositions)
            {
                _flagsPositions[flagPosition.Key] = false;
            }
        }
    }
}
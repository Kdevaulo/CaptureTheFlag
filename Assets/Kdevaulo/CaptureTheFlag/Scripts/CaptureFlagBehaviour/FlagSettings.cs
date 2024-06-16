using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;

namespace Kdevaulo.CaptureTheFlag.CaptureFlagBehaviour
{
    [CreateAssetMenu(menuName = nameof(CaptureFlagBehaviour) + "/" + nameof(FlagSettings),
        fileName = nameof(FlagSettings))]
    public class FlagSettings : ScriptableObject, IReinitializable
    {
        [field: Min(0)]
        [field: SerializeField] public int FlagsCount { get; private set; }

        [field: Min(0)]
        [field: SerializeField] public float CursorSpeed { get; private set; }
        [field: Min(0)]
        [field: SerializeField] public float FlagRadiusInUnits { get; private set; }
        [field: Min(0)]
        [field: SerializeField] public float OnLoseDelay { get; private set; }
        [field: Min(0)]
        [field: SerializeField] public float SecondsToCapture { get; private set; }
        [field: Range(0, 1)]
        [field: SerializeField] public float MiniGameChance { get; private set; }

        [field: SerializeField] public Vector3[] SpawnPositions { get; private set; }

        private Dictionary<Vector3, bool> _flagsPositions;

        void IReinitializable.Reinitialize()
        {
            Initialize();
        }

        public void Initialize()
        {
            _flagsPositions = new Dictionary<Vector3, bool>(SpawnPositions.Length);

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
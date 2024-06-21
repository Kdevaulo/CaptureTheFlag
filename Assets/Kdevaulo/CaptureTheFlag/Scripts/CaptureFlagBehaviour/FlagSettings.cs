using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

namespace Kdevaulo.CaptureTheFlag.CaptureFlagBehaviour
{
    [CreateAssetMenu(menuName = nameof(CaptureFlagBehaviour) + "/" + nameof(FlagSettings),
        fileName = nameof(FlagSettings))]
    public class FlagSettings : ScriptableObject
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

        private List<PositionData> _flagsPositions;

        public void Initialize()
        {
            _flagsPositions = new List<PositionData>(SpawnPositions.Length);

            foreach (var position in SpawnPositions)
            {
                _flagsPositions.Add(new PositionData(position, false));
            }
        }

        public Vector3 GetPosition()
        {
            var targetPosition = BorrowFirstFreePosition();

            return targetPosition;
        }

        private Vector3 BorrowFirstFreePosition()
        {
            var freePosition = TryGetFreePosition();

            if (freePosition == null)
            {
                RefreshFlags();
            }
            else
            {
                freePosition.IsBusy = true;

                return freePosition.Position;
            }

            freePosition = TryGetFreePosition();
            Assert.IsNotNull(freePosition);

            return freePosition.Position;
        }

        private PositionData TryGetFreePosition()
        {
            return _flagsPositions.FirstOrDefault(x => !x.IsBusy);
        }

        private void RefreshFlags()
        {
            _flagsPositions.ForEach(x => x.IsBusy = false);
        }
    }
}
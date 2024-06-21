using System;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.MiniGameBehaviour
{
    [Serializable]
    public struct MiniGameModel
    {
        public string Guid { get; private set; }
        public float Position { get; private set; }

        private readonly float _correctAreaSize;

        private float _timeLeft;
        private float _movementSpeed;
        private float _correctPosition;

        public MiniGameModel(MiniGameData data)
        {
            Guid = data.Guid;
            Position = 0;
            _correctPosition = data.CorrectPosition;

            _timeLeft = data.Duration;
            _movementSpeed = data.MovementSpeed;
            _correctAreaSize = data.CorrectAreaSize;
        }

        public bool CheckPosition()
        {
            float halfArea = _correctAreaSize / 2;

            bool isInArea = Position > _correctPosition - halfArea && Position < _correctPosition + halfArea;

            return isInArea;
        }

        public bool IsTimeOver()
        {
            return _timeLeft <= 0;
        }

        public void Move(float deltaTime)
        {
            // note: necessary for moving within specified boundaries and avoiding bugs
            if (Position + _movementSpeed * deltaTime > 1)
            {
                _movementSpeed = -_movementSpeed;
            }

            if (Position + _movementSpeed * deltaTime < 0)
            {
                _movementSpeed = -_movementSpeed;
            }

            Position = Mathf.Clamp01(Position + _movementSpeed * deltaTime);

            _timeLeft -= deltaTime;
        }
    }
}
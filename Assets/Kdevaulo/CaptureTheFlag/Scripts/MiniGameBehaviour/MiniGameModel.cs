﻿using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.MiniGameBehaviour
{
    public class MiniGameModel
    {
        public IFlagInvader Invader { get; private set; }
        public IMiniGameObserver Observer { get; private set; }

        public float Position { get; private set; }

        private readonly float _correctAreaSize;
        private readonly float _correctPosition;

        private float _timeLeft;
        private float _movementSpeed;

        public MiniGameModel(IMiniGameObserver observer, IFlagInvader invader, float timeLeft, float correctPosition,
            float movementSpeed, float correctAreaSize)
        {
            Invader = invader;
            Observer = observer;

            _timeLeft = timeLeft;
            _movementSpeed = movementSpeed;
            _correctAreaSize = correctAreaSize;
            _correctPosition = correctPosition;
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
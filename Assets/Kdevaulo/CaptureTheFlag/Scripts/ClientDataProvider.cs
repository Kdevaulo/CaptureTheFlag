﻿using System;

using Kdevaulo.CaptureTheFlag.MiniGameBehaviour;

using Mirror;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag
{
    [AddComponentMenu(nameof(ClientDataProvider) + " in " + nameof(CaptureTheFlag))]
    public class ClientDataProvider : MonoBehaviour
    {
        public event Action MovableSet = delegate { };

        public IMovable Movable { get; private set; }

        private IMiniGameClientInitializer _initializer;
        private IMiniGameEventsHandler _eventsHandler;

        public void Initialize(IMiniGameClientInitializer initializer, IMiniGameEventsHandler eventsHandler)
        {
            _initializer = initializer;

            _eventsHandler = eventsHandler;
        }

        [Client]
        public void SetMovable(IMovable movable)
        {
            Movable = movable;

            MovableSet.Invoke();
        }

        [Client]
        public void InitializeMiniGame(MiniGameData data)
        {
            _initializer.InitializeMiniGame(data);
        }

        [Client]
        public IMiniGameEventsHandler GetEventsHandler()
        {
            return _eventsHandler;
        }
    }
}
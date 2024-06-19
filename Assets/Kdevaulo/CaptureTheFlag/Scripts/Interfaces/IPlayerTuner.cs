using System;

using Kdevaulo.CaptureTheFlag.PlayerBehaviour;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag
{
    public interface IPlayerTuner
    {
        event Action<Color, IPlayer> PlayerTuned;

        void TunePlayer(PlayerView view);
    }
}
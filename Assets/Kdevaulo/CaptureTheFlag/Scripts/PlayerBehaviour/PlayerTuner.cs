using System;

using Mirror;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.PlayerBehaviour
{
    public class PlayerTuner : IPlayerTuner
    {
        public event Action<Color, IPlayer> PlayerTuned = delegate { };

        private readonly IColorProvider _colorProvider;

        public PlayerTuner(IColorProvider colorProvider)
        {
            _colorProvider = colorProvider;
        }

        [Server]
        public void TunePlayer(PlayerView view)
        {
            var color = _colorProvider.GetColor();

            view.SetColor(color);

            PlayerTuned.Invoke(color, view);
        }
    }
}
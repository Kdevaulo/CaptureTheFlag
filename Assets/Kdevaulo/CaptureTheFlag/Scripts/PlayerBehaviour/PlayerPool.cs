using System.Collections.Generic;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.PlayerBehaviour
{
    public class PlayerPool
    {
        private readonly PlayerFactory _factory;

        private readonly int _initialSize = 3;
        private readonly PlayerSettings _settings;

        private int _lastColorIndex;

        private Dictionary<PlayerView, bool> _viewsWithStates;

        public PlayerPool(PlayerFactory factory, PlayerSettings settings)
        {
            _factory = factory;
            _settings = settings;

            _viewsWithStates = new Dictionary<PlayerView, bool>(_initialSize);
        }

        public void Clear()
        {
            foreach (var view in _viewsWithStates.Keys)
            {
                Object.Destroy(view.gameObject);
            }

            _viewsWithStates.Clear();
        }

        public void Initialize()
        {
            for (int index = 0; index < _initialSize; index++)
            {
                //var targetColor = _settings.GetColor();
                //var view = AddItem(targetColor);
                //view.SetDefaultPosition();
            }
        }

        public void Return(PlayerView view)
        {
            _viewsWithStates[view] = false;
            view.Disable();
            view.SetDefaultPosition();
        }

        // public PlayerView Get()
        // {
        //     foreach (var item in _viewsWithStates)
        //     {
        //         if (!item.Value)
        //         {
        //             var view = item.Key;
        //             _viewsWithStates[view] = true;
        //
        //             view.Enable();
        //             return view;
        //         }
        //     }
        //
        //     var color = _settings.GetColor();
        //
        //     return AddItem(color, true);
        // }

        private PlayerView AddItem(Color targetColor, bool active = false)
        {
            var view = _factory.Create(targetColor);
            _viewsWithStates.Add(view, active);

            if (active)
            {
                view.Enable();
            }
            else
            {
                view.Disable();
            }

            return view;
        }
    }
}
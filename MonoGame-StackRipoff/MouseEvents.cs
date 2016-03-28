using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGame_StackRipoff
{
    public class MouseEvents
    {
        private readonly GameWindow _window;
        private readonly MouseButtonEvents _right = new MouseButtonEvents();
        private readonly MouseButtonEvents _left = new MouseButtonEvents();
        private readonly MouseButtonEvents _middle = new MouseButtonEvents();

        public MouseEvents(GameWindow window)
        {
            _window = window;
        }

        public void Update(GameTime gameTime)
        {
            var state = Mouse.GetState(_window);
            _right.Update(state.RightButton, state.X, state.Y);
            _left.Update(state.LeftButton, state.X, state.Y);
            _middle.Update(state.MiddleButton, state.X, state.Y);
        }

        public void OnLeftClick(Action<int, int> handler)
        {
            _left.OnClick(handler);
        }

        public void OnRightClick(Action<int, int> handler)
        {
            _right.OnClick(handler);
        }

        public void OnMiddleClick(Action<int, int> handler)
        {
            _middle.OnClick(handler);
        }

        private class MouseButtonEvents
        {
            private ButtonState _lastState = ButtonState.Released;
            private readonly List<Action<int, int>> _handlers = new List<Action<int, int>>();

            public void Update(ButtonState nextState, int x, int y)
            {
                if (_lastState == ButtonState.Released)
                {
                    if (nextState != ButtonState.Pressed)
                    {
                        return;
                    }
                    foreach (var h in _handlers)
                    {
                        h(x, y);
                    }
                    _lastState = ButtonState.Pressed;
                }
                else if (nextState == ButtonState.Released)
                {
                    _lastState = ButtonState.Released;
                }
            }

            public void OnClick(Action<int, int> handler)
            {
                _handlers.Add(handler);
            }
        }
    }
}

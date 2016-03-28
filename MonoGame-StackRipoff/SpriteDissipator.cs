using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_StackRipoff
{
    public enum SpriteDissipatorState
    {
        Ready,
        Running,
        Finished
    }
    public class SpriteDissipator
    {
        public SpriteDissipatorState State { get; private set; }
        private readonly Texture2D _texture;
        private readonly Vector2 _center;
        private Vector2 _upLeft;
        private Vector2 _upRight;
        private Vector2 _downLeft;
        private Vector2 _downRight;
        private float _outerOpacity = 0.5f;
        private float _centerOpacity = 1;

        private readonly Animator[] _animators;

        public SpriteDissipator(Texture2D texture, Vector2 position)
        {
            State = SpriteDissipatorState.Ready;
            _texture = texture;
            _center = _upLeft = _upRight = _downLeft = _downRight = position;
            const float duration = 1f;
            int movementDelta = _texture.Width / 10;
            _animators = new[]{
                new Animator(Easing.Linear, _center.X, val => _upLeft.X = val, -movementDelta, duration), 
                new Animator(Easing.Linear, _center.Y, val => _upLeft.Y = val, -movementDelta, duration), 
                new Animator(Easing.Linear, _center.X, val => _upRight.X = val, movementDelta, duration), 
                new Animator(Easing.Linear, _center.Y, val => _upRight.Y = val, -movementDelta, duration), 
                new Animator(Easing.Linear, _center.X, val => _downLeft.X = val, -movementDelta, duration), 
                new Animator(Easing.Linear, _center.Y, val => _downLeft.Y = val, movementDelta, duration), 
                new Animator(Easing.Linear, _center.X, val => _downRight.X = val, movementDelta, duration), 
                new Animator(Easing.Linear, _center.Y, val => _downRight.Y = val, movementDelta, duration), 
                new Animator(Easing.Linear, _outerOpacity, val => _outerOpacity = val, -0.5f, duration), 
                new Animator(Easing.Linear, _centerOpacity, val => _centerOpacity = val, -1f, duration), 
            };
        }

        public void Start()
        {
            if (State != SpriteDissipatorState.Ready)
            {
                return;
            }
            State = SpriteDissipatorState.Running;
        }

        public void Reset()
        {
            if (State != SpriteDissipatorState.Finished)
            {
                return;
            }
            var zeroGameTime = new GameTime();
            foreach (var a in _animators)
            {
                a.Reset();
                a.Update(zeroGameTime);
            }
            State = SpriteDissipatorState.Ready;
        }

        public void Update(GameTime gameTime)
        {
            if (State != SpriteDissipatorState.Running)
            {
                return;
            }
            foreach (var a in _animators)
            {
                a.Update(gameTime);
            }
            if (!_animators.All(a => a.Finished))
            {
                return;
            }
            State = SpriteDissipatorState.Finished;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (State)
            {
                case SpriteDissipatorState.Finished:
                    return;
                case SpriteDissipatorState.Ready:
                    spriteBatch.Draw(_texture, _center, Color.White * _centerOpacity);
                    break;
                case SpriteDissipatorState.Running:
                    spriteBatch.Draw(_texture, _center, Color.White * _centerOpacity);
                    foreach (var position in new[] { _upLeft, _upRight, _downLeft, _downRight })
                    {
                        spriteBatch.Draw(_texture, position, Color.White * _outerOpacity);
                    }
                    break;
            }
        }
    }
}
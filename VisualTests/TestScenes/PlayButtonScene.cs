using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_StackRipoff;

namespace VisualTests.TestScenes
{
    public class PlayButtonScene  : Scene
    {
        private readonly Texture2D _texture;
        private readonly Vector2 _center;
        private Vector2 _upLeft;
        private Vector2 _upRight;
        private Vector2 _downLeft;
        private Vector2 _downRight;
        private float _outerOpacity = 0.5f;
        private float _centerOpacity = 1;

        private readonly Animator[] _animators;

        public PlayButtonScene(GraphicsDevice graphics, ContentManager content) : base(graphics)
        {
            _texture = content.Load<Texture2D>("PlayButton");
            _center = Graphics.Viewport.GetCenter() - new Vector2(_texture.Width, _texture.Height)/2f;
            _upLeft = _upRight = _downLeft = _downRight = _center;
            const float duration = 1f;
            int movementDelta = _texture.Width/10;
            _animators = new []{
                new Animator(Easing.Linear, _upLeft.X, val => _upLeft.X = val, -movementDelta, duration), 
                new Animator(Easing.Linear, _upLeft.Y, val => _upLeft.Y = val, -movementDelta, duration), 
                new Animator(Easing.Linear, _upRight.X, val => _upRight.X = val, movementDelta, duration), 
                new Animator(Easing.Linear, _upRight.Y, val => _upRight.Y = val, -movementDelta, duration), 
                new Animator(Easing.Linear, _downLeft.X, val => _downLeft.X = val, -movementDelta, duration), 
                new Animator(Easing.Linear, _downLeft.Y, val => _downLeft.Y = val, movementDelta, duration), 
                new Animator(Easing.Linear, _downRight.X, val => _downRight.X = val, movementDelta, duration), 
                new Animator(Easing.Linear, _downRight.Y, val => _downRight.Y = val, movementDelta, duration), 
                new Animator(Easing.Linear, _outerOpacity, val => _outerOpacity = val, -0.5f, duration), 
                new Animator(Easing.Linear, _centerOpacity, val => _centerOpacity = val, -1f, duration), 
            };
        }

        public override void DrawSprites(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw( _texture, _center, Color.White * _centerOpacity);
            foreach (var position in new[] {_upLeft, _upRight, _downLeft, _downRight})
            {
                spriteBatch.Draw( _texture, position, Color.White * _outerOpacity);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var a in _animators)
            {
                a.Update(gameTime);
            }
            if (!_animators.All(a => a.Finished))
            {
                return;
            }
            foreach (var a in _animators)
            {
                a.Reset();
            }
        }
    }
}

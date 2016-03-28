using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_StackRipoff;

namespace VisualTests.TestScenes
{
    public class ParticlesScene : Scene
    {
        private readonly EasyTimer _timer = new EasyTimer(TimeSpan.FromSeconds(0.5f));
        private readonly ParticleSystem _particleSystem;
        public ParticlesScene(GraphicsDevice graphics, ContentManager content) : base(graphics)
        {
            _particleSystem = new ParticleSystem(Graphics, content);
        }

        public override void DrawSprites(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _particleSystem.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            _particleSystem.Update(gameTime);
            if (_timer.IsFinished(gameTime))
            {
                _particleSystem.SpawnRandom();
                _timer.Reset(gameTime);
            }
        }
    }
}
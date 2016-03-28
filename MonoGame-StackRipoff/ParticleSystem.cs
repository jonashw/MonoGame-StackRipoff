using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_StackRipoff
{
    public class ParticleSystem
    {
        private readonly List<Particle> _particles = new List<Particle>();
        private readonly GraphicsDevice _graphics;
        private readonly Texture2D _texture;
        public ParticleSystem(GraphicsDevice graphics, ContentManager content)
        {
            _graphics = graphics;
            _texture = content.Load<Texture2D>("Particle");
        }

        public void Update(GameTime gameTime)
        {
            foreach (var particle in _particles)
            {
                particle.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var p in _particles)
            {
                p.Draw(spriteBatch);
            }
        }

        public void SpawnRandom()
        {
            var r = new Random();
            var x = r.Next(0, _graphics.Viewport.Width);
            var y = r.Next(0, _graphics.Viewport.Height);
            var particle = new Particle(_texture, new Vector2(x, y), r.Next(0, 90));
            _particles.Add(particle);
        }

        private class Particle
        {
            private readonly Texture2D _texture;
            private readonly Vector2 _position;
            private readonly float _rotation;
            private float _opacity = 1;
            private readonly Animator _animator;

            public Particle(Texture2D texture, Vector2 position, float rotation)
            {
                _texture = texture;
                _position = position;
                _rotation = rotation;
                _animator = new Animator(Easing.CubicInOut, 1, val => _opacity = val, -1f, 2);
            }

            public void Update(GameTime gameTime)
            {
                _animator.Update(gameTime);
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(_texture, _position, rotation: _rotation, color: Color.White * _opacity);
            }

            public bool Finished
            {
                get { return _animator.Finished; }
            }
        }
    }
}
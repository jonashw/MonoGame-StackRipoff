using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_StackRipoff
{
    public class ParticleSystem
    {
        private readonly List<Particle> _particles = new List<Particle>();
        private readonly List<Particle> _transferList = new List<Particle>();
        private readonly GraphicsDevice _graphics;
        private readonly Texture2D _texture;
        public ParticleSystem(GraphicsDevice graphics, ContentManager content)
        {
            _graphics = graphics;
            _texture = content.Load<Texture2D>("Particle");
        }

        public void Update(GameTime gameTime)
        {
            //Take out the garbage.... later this will be replaced with an object pool.
            foreach (var particle in _particles)
            {
                particle.Update(gameTime);
                if (particle.State != ParticleState.Finished)
                {
                    _transferList.Add(particle);
                }
            }
            _particles.Clear();
            foreach (var p in _transferList)
            {
                _particles.Add(p);
            }
            _transferList.Clear();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var p in _particles)
            {
                p.Draw(spriteBatch);
            }
        }

        public void SpawnRandom(uint count = 1)
        {
            var r = new Random();
            foreach (var i in Enumerable.Range(0, (int)count))
            {
                var x = r.Next(0, _graphics.Viewport.Width);
                var y = r.Next(0, _graphics.Viewport.Height);
                var particle = new Particle(
                    _texture,
                    new Vector2(x, y),
                    r.Next(0, 90),
                    r.Next(0, (int) MathHelper.TwoPi));
                _particles.Add(particle);
            }
        }

        private enum ParticleState
        {
            Entering,
            HangingOut,
            Exiting,
            Finished
        }

        private class Particle
        {
            private readonly Texture2D _texture;
            private Vector2 _position;
            private readonly float _rotation;
            private float _opacity = 0;
            private float _scale = 0;
            private readonly Animator[] _movementAnimators;
            private readonly Animator[] _enteringAnimators;
            private readonly Animator[] _exitingAnimators;
            public ParticleState State { get; private set; }
            private readonly EasyTimer _hangingOutTimer;

            public Particle(Texture2D texture, Vector2 position, float rotation, float directionInRadians)
            {
                _texture = texture;
                _position = position;
                _rotation = rotation;
                State = ParticleState.Entering;

                const float movementDelta = 100f;
                var dX = (float)(movementDelta*Math.Cos(directionInRadians));
                var dY = (float)(movementDelta*Math.Sin(directionInRadians));
                _movementAnimators = new[]
                {
                    new Animator(Easing.Linear, position.X, val => _position.X = val, dX, 6),
                    new Animator(Easing.Linear, position.Y, val => _position.Y = val, dY, 6)
                };
                _enteringAnimators = new []
                {
                    new Animator(Easing.CubicInOut, 0, val => _opacity = val, 1f, 2),
                    new Animator(Easing.CircularInOut, 0, val => _scale = val, 1f, 2),
                };
                _exitingAnimators = new []
                {
                    new Animator(Easing.CubicInOut, 1, val => _opacity = val, -1f, 2),
                    new Animator(Easing.CircularInOut, 1, val => _scale = val, -1f, 2),
                };
                _hangingOutTimer =  new EasyTimer(TimeSpan.FromSeconds(1));
            }

            public void Update(GameTime gameTime)
            {
                if (State == ParticleState.Finished)
                {
                    return;
                }
                foreach (var a in _movementAnimators)
                {
                    a.Update(gameTime);
                }
                switch (State)
                {
                    case ParticleState.Entering:
                        foreach (var a in _enteringAnimators)
                        {
                            a.Update(gameTime);
                        }
                        if (_enteringAnimators.All(a => a.Finished))
                        {
                            State = ParticleState.HangingOut;
                            _hangingOutTimer.Reset(gameTime);
                        }
                        break;
                    case ParticleState.HangingOut:
                        if (_hangingOutTimer.IsFinished(gameTime))
                        {
                            State = ParticleState.Exiting;
                        }
                        break;
                    case ParticleState.Exiting:
                        foreach (var a in _exitingAnimators)
                        {
                            a.Update(gameTime);
                        }
                        if (_exitingAnimators.All(a => a.Finished))
                        {
                            State = ParticleState.Finished;
                        }
                        break;
                }
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                if (State == ParticleState.Finished)
                {
                    return;
                }
                spriteBatch.Draw(
                    _texture,
                    _position,
                    rotation: _rotation,
                    color: Color.White * _opacity,
                    scale: new Vector2(_scale, _scale),
                    origin: new Vector2(_texture.Width/2f, _texture.Height/2f));
            }
        }
    }
}
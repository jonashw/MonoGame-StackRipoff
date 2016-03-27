using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_StackRipoff
{
    public class MultiRectangularRingAnimation : IRectangularRingAnimation
    {
        private readonly IRectangularRingAnimation[] _animations;
        private Vector3 _position;
        public MultiRectangularRingAnimation(IRectangularRingAnimation[] animations, Vector3? position = null)
        {
            _animations = animations;
            if (position.HasValue)
            {
                _position = position.Value;
            }
            foreach (var b in animations)
            {
                b.Position = _position;
            }
        }
        public Vector3 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                foreach (var burst in _animations)
                {
                    burst.Position = value;
                }
            }
        }

        public bool Finished
        {
            get { return _animations.All(b => b.Finished); }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var burst in _animations)
            {
                burst.Update(gameTime);
            }
        }

        public void Draw(GraphicsDevice graphics, BasicEffect effect)
        {
            foreach (var burst in _animations)
            {
                burst.Draw(graphics,effect);
            }
        }

        public void Reset()
        {
            foreach (var burst in _animations)
            {
                burst.Reset();
            }
        }

        public static MultiRectangularRingAnimation Create(float sizeX, float sizeZ, int count, Vector3? position = null)
        {
            return new MultiRectangularRingAnimation(
                Enumerable.Range(0, Math.Abs(count)).Select(i =>
                {
                    var offset = 2*i*RectangularRingAnimation.Width;
                    return (IRectangularRingAnimation)
                        new RectangularRingAnimation(
                            sizeX - offset,
                            sizeZ - offset);
                }).Concat(new []
                {
                    new RectangularRingHalo(sizeX, sizeZ) 
                }).ToArray(), position);
        }
    }
}
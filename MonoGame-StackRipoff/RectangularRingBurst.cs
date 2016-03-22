using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_StackRipoff
{
    public class RectangularRingBurst
    {
        private readonly RectangularRing _ring;
        private readonly Animator _animator;

        public RectangularRingBurst(float innerSizeX, float innerSizeZ)
        {
            _ring = new RectangularRing
            {
                InnerSizeX = innerSizeX,
                InnerSizeZ = innerSizeZ,
                OuterSize = 0.25f
            };
            _animator = new Animator(Easing.CubicOut, innerSizeX, v =>
            {
                _ring.InnerSizeX = v;
                _ring.InnerSizeZ = v;
            }, 2f*innerSizeX, 1);
        }

        public void Update(GameTime gameTime)
        {
            _animator.Update(gameTime);
        }

        public void Draw(GraphicsDevice graphics, BasicEffect effect)
        {
            _ring.Draw(graphics, effect);
        }

        public bool Finished
        {
            get { return _animator.Finished; }
        }

        public void Reset()
        {
            _animator.Reset();
        }
    }
}
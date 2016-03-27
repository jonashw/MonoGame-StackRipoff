using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_StackRipoff
{
    public class RectangularRingAnimation : IRectangularRingAnimation
    {
        private readonly RectangularRing _ring;
        private readonly Animator[] _animators;
        public const float Width = 0.25f;

        public Vector3 Position
        {
            get { return _ring.Position; }
            set { _ring.Position = value; }
        }
        
        public RectangularRingAnimation(float innerSizeX, float innerSizeZ)
        {
            _ring = new RectangularRing
            {
                InnerSizeX = innerSizeX,
                InnerSizeZ = innerSizeZ,
                OuterSize = Width
            };
            _animators = new[]
            {
                new Animator(Easing.CubicOut, innerSizeX, v =>
                {
                    _ring.InnerSizeX = v;
                }, innerSizeX/2f, 1),

                new Animator(Easing.CubicOut, innerSizeZ, v =>
                {
                    _ring.InnerSizeZ = v;
                }, innerSizeZ/2f, 1),

                new Animator(Easing.CubicOut, 1, o =>
                {
                    _ring.Opacity = o;
                }, -1, 1.5f)
            };
        }

        public void Update(GameTime gameTime)
        {
            foreach (var a in _animators)
            {
                a.Update(gameTime);
            }
        }

        public void Draw(GraphicsDevice graphics, BasicEffect effect)
        {
            _ring.Draw(graphics, effect);
        }

        public bool Finished
        {
            get { return _animators.All(a => a.Finished); }
        }

        public void Reset()
        {
            foreach (var a in _animators)
            {
                a.Reset();
            }
        }
    }
}
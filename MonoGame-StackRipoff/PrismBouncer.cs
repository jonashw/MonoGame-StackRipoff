using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_StackRipoff
{
    public class PrismBouncer
    {
        public bool Enabled;
        public RectangularPrism Prism;
        public readonly float BounceSize;
        private readonly Animator _animator;

        private bool _usingX = true;

        public PrismBounceAxis CurrentAxis
        {
            get { return _usingX ? PrismBounceAxis.X : PrismBounceAxis.Z; }
        }

        public void ToggleDirection()
        {
            _usingX = !_usingX;
        }

        private void setValue(float val)
        {
            if (_usingX)
            {
                Prism.Position.X = val;
            }
            else
            {
                Prism.Position.Z = -val;
            }
        }

        private float getValue()
        {
            return _usingX ? Prism.Position.X : -Prism.Position.Z;
        }

        public PrismBouncer(RectangularPrism prism, uint bounceSize)
        {
            Prism = prism;
            BounceSize = 0.5f*bounceSize;
            _animator = new Animator(
                Easing.Linear,
                BounceSize,
                setValue,
                -bounceSize,
                2);
            Enabled = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!Enabled)
            {
                return;
            }
            _animator.Update(gameTime);
            if (!_animator.Finished)
            {
                return;
            }
            _animator.StartingValue = getValue();
            _animator.ValueChange = -_animator.ValueChange;
            _animator.Reset();
        }

        public void Reset()
        {
            if (!Enabled)
            {
                return;
            }
            _animator.StartingValue = BounceSize;
            _animator.ValueChange = -2f*BounceSize;
            _animator.Reset();
        }

        public void Draw(GraphicsDevice graphics, BasicEffect effect)
        {
            if (!Enabled)
            {
                return;
            }
            Prism.Draw(graphics, effect);
        }
    }
}
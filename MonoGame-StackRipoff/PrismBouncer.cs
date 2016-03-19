using Microsoft.Xna.Framework;

namespace MonoGame_StackRipoff
{
    public class PrismBouncer
    {
        public RectangularPrism Prism;
        public readonly float BounceSize;
        private readonly Animator _animator;

        private bool _usingX = true;

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
        }

        public void Update(GameTime gameTime)
        {
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
            _animator.StartingValue = BounceSize;
            _animator.ValueChange = -2f*BounceSize;
            _animator.Reset();
        }
    }
}
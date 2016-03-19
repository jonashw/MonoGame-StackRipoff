using System;
using Microsoft.Xna.Framework;

namespace MonoGame_StackRipoff
{
    public class Animator
    {
        public Easing.EasingFn Easing;
        public double Duration;
        public double StartingValue;
        public float ValueChange;
        private readonly Action<float> _setter;
        private double _currentAnimationTime;
        public bool Finished { get; private set; }

        public Animator(Easing.EasingFn easing, float startingValue, Action<float> setter, float valueChange, double duration)
        {
            Easing = easing;
            ValueChange = valueChange;
            StartingValue = startingValue;
            Duration = duration;
            _setter = setter;
        }

        public void Update(GameTime gameTime)
        {
            if (Finished)
            {
                return;
            }
            _currentAnimationTime = Math.Min(
                _currentAnimationTime + gameTime.ElapsedGameTime.TotalSeconds,
                Duration); // Prevent over-animating

            var nextValue = (float)Easing(
                _currentAnimationTime,
                StartingValue,
                ValueChange,
                Duration);

            _setter(nextValue);

            if (_currentAnimationTime >= Duration)
            {
                Finished = true;
            }
        }

        public void Reset(float? newStartingValue = null)
        {
            if (newStartingValue.HasValue)
            {
                StartingValue = newStartingValue.Value;
            }
            _currentAnimationTime = 0;
            Finished = false;
        }
    }
}
using System;
using Microsoft.Xna.Framework;

namespace MonoGame_StackRipoff
{
    public static class PlayRegistry
    {
        private static int _streak = 0;
        public static IRectangularRingAnimation Perfect(PrismOverlapResult.PerfectLanding result)
        {
            _streak++;
            if (_streak >= 8)
            {
                _streak = 1;
            }
            var position = new Vector3(
                result.Landed.Position.X,
                result.Landed.Bottom,
                result.Landed.Position.Z);
            if (_streak < 3)
            {
                return new RectangularRingHalo(result.Landed.Size.X, result.Landed.Size.Z)
                {
                    Position = position
                };
            }
            return MultiRectangularRingAnimation.Create(
                result.Landed.Size.X,
                result.Landed.Size.Z,
                Math.Min(_streak-2,3),
                position);
        }

        public static void NotPerfect()
        {
            _streak = 0;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame_StackRipoff.MonoGameInscribedTriangles;

namespace MonoGame_StackRipoff
{
    public static class SceneColors
    {
        private static readonly List<Color> ColorArray = new List<Color>
        {
            new Color(242, 116, 119),
            new Color(222, 109, 39),
            new Color(254, 214, 82),
            new Color(253, 235, 51),
            new Color(209, 227, 143),
            new Color(21, 152, 98),
            new Color(0, 138, 177),
            new Color(40, 92, 168),
            new Color(133, 90, 162),
            new Color(216, 117, 172)
        };

        private static readonly CircularArray<Tuple<Color, Color>> ColorPairs =
            new CircularArray<Tuple<Color, Color>>(
                ColorArray.Zip(
                    ColorArray.Skip(1).Concat(new[] {ColorArray.First()}),
                    Tuple.Create).ToArray());

        private static float _transitionProgress;
        public static Color NextPrismColor()
        {
            _transitionProgress += 0.125f;
            if (_transitionProgress >= 1)
            {
                _transitionProgress = 0f;
                ColorPairs.Next();
            }
            var pair = ColorPairs.GetCurrent();
            return Color.Lerp(pair.Item1, pair.Item2, _transitionProgress);
        }
    }
}
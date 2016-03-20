using Microsoft.Xna.Framework;
using MonoGame_StackRipoff.MonoGameInscribedTriangles;

namespace MonoGame_StackRipoff
{
    public static class SceneColors
    {
        private static readonly CircularArray<Color> Colors = new CircularArray<Color>(
            new Color(242,116,119),
            new Color(222,109,39),
            new Color(254,214,82),
            new Color(253,235,51),
            new Color(209,227,143),
            new Color(21,152,98),
            new Color(0,138,177),
            new Color(40,92,168),
            new Color(133,90,162),
            new Color(216,117,172));
        public static Color NextPrismColor()
        {
            var color = Colors.GetCurrent();
            Colors.Next();
            return color;
        }
    }
}
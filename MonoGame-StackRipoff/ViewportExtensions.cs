using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_StackRipoff
{
    public static class ViewPortExtensions
    {
        public static Vector2 GetCenter(this Viewport vp)
        {
            return new Vector2(vp.Width / 2f, vp.Height / 2f);
        }
    }
}
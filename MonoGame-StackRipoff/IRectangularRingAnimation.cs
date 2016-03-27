using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_StackRipoff
{
    public interface IRectangularRingAnimation
    {
        Vector3 Position { get; set; }
        bool Finished { get; }
        void Update(GameTime gameTime);
        void Draw(GraphicsDevice graphics, BasicEffect effect);
        void Reset();
    }
}
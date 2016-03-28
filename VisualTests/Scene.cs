using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VisualTests
{
    public class Scene
    {
        protected readonly GraphicsDevice Graphics;
        protected Scene(GraphicsDevice graphics)
        {
            Graphics = graphics;
        }
        public virtual void Update(GameTime gameTime) { }
        public virtual void DrawSprites(SpriteBatch spriteBatch, GameTime gameTime) { }
        public virtual void DrawOther(GameTime gameTime) { }
    }
}
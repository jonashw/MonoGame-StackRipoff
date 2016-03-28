using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame_StackRipoff
{
    public class Cursor
    {
        private readonly Texture2D _texture;

        public Cursor(Texture2D texture)
        {
            _texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var position = Mouse.GetState().Position.ToVector2() - new Vector2(_texture.Width, _texture.Height)/2f;
            spriteBatch.Draw(_texture, position);
        }
    }
}
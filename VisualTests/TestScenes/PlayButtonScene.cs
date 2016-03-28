using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_StackRipoff;

namespace VisualTests.TestScenes
{
    public class PlayButtonScene : Scene
    {
        private readonly SpriteDissipator _playButton;

        public PlayButtonScene(GraphicsDevice graphics, ContentManager content) : base(graphics)
        {
            var texture = content.Load<Texture2D>("PlayButton");
            _playButton = new SpriteDissipator(
                texture,
                Graphics.Viewport.GetCenter() - new Vector2(texture.Width, texture.Height)/2f);
            _playButton.Start();
        }

        public override void DrawSprites(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _playButton.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            _playButton.Update(gameTime);
            if (_playButton.State == SpriteDissipatorState.Finished)
            {
                _playButton.Reset();
            }
        }
    }
}
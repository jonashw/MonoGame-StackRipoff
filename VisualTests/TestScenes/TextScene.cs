﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_StackRipoff;

namespace VisualTests.TestScenes
{
    public class TextScene : Scene
    {
        private readonly SpriteFont _font;

        public TextScene(GraphicsDevice graphics, ContentManager content) : base(graphics)
        {
            _font = content.Load<SpriteFont>("Consolas");
        }

        public override void DrawSprites(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var str = "This is a test";
            var strSize = _font.MeasureString(str);
            spriteBatch.DrawString(_font, str, Graphics.Viewport.GetCenter() - (strSize/2f), Color.White);
        }
    }
}
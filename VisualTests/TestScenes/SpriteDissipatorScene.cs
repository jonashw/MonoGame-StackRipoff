using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_StackRipoff;

namespace VisualTests.TestScenes
{
    public class SpriteDissipatorScene : Scene
    {
        private readonly SpriteDissipator[] _dissipators;

        public SpriteDissipatorScene(GraphicsDevice graphics, ContentManager content) : base(graphics)
        {
            var texture = content.Load<Texture2D>("PlayButton");
            var vp = new Vector2(Graphics.Viewport.Width, Graphics.Viewport.Height/2f);
            _dissipators = new[]
            {
                createAndStartDissipator(content.Load<Texture2D>("PlayButton"), new Vector2(vp.X/3f, vp.Y)),
                createAndStartDissipator(content.Load<Texture2D>("Logo"), new Vector2(2*vp.X/3f, vp.Y))
            };
        }

        private static SpriteDissipator createAndStartDissipator(Texture2D texture, Vector2 position)
        {
            var sd = new SpriteDissipator(
                texture,
                centerTextureAboutPosition(texture, position));
            sd.Start();
            return sd;
        }

        private static Vector2 centerTextureAboutPosition(Texture2D texture, Vector2 position)
        {
            return position - new Vector2(texture.Width, texture.Height)/2f;
        }

        public override void DrawSprites(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var d in _dissipators)
            {
                d.Draw(spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var d in _dissipators)
            {
                d.Update(gameTime);
            }
            if (_dissipators.All(d => d.State != SpriteDissipatorState.Finished))
            {
                return;
            }
            foreach (var d in _dissipators)
            {
                d.Reset();
            }
        }
    }
}
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_StackRipoff;

namespace VisualTests
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private readonly IEnumerable<RectangularRingBurst> _bursts = new[]
        {
            new RectangularRingBurst(0.5f, 0.5f), 
            new RectangularRingBurst(1.0f, 1.0f), 
            new RectangularRingBurst(1.5f, 1.5f), 
        };

        private readonly IEnumerable<Animator> _ringAnimators;
        private readonly RasterizerState _rasterizerState = new RasterizerState
        {
            CullMode = CullMode.CullClockwiseFace
        };

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _effect = new BasicEffect(GraphicsDevice)
            {
                AmbientLightColor = Vector3.One,
                LightingEnabled = true,
                DiffuseColor = Vector3.One
            };
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (var burst in _bursts)
            {
                burst.Update(gameTime);
                if (burst.Finished)
                {
                    burst.Reset();
                }
            }

            base.Update(gameTime);
        }

        private BasicEffect _effect;

        protected override void Draw(GameTime gameTime)
        {
            _effect.EnableDefaultLighting();
            GraphicsDevice.RasterizerState = _rasterizerState;
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _effect.View =
                Matrix.CreateRotationY(MathHelper.ToRadians(45))
                *Matrix.CreateRotationX(MathHelper.ToRadians(45))
                *Matrix.CreateTranslation(0f, 0, -400f);

            _effect.Projection = Matrix.CreateOrthographic(
                GraphicsDevice.Viewport.Width/25f,
                GraphicsDevice.Viewport.Height/25f,
                0.1f,
                500f);

            foreach (var burst in _bursts)
            {
                burst.Draw(GraphicsDevice, _effect);
            }

            base.Draw(gameTime);
        }
    }
}
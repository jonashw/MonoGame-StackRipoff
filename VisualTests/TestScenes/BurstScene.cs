using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_StackRipoff;

namespace VisualTests.TestScenes
{
    public class BurstScene : Scene
    {
        private readonly BasicEffect _effect;
        private readonly IEnumerable<IRectangularRingAnimation> _bursts = new[]
        {
            (IRectangularRingAnimation) new RectangularRingHalo(4f, 4f){Position = new Vector3(-6, 0, 0)},
            new RectangularRingAnimation(4f, 4f){Position = new Vector3(-2, 0, 0)},
            MultiRectangularRingAnimation.Create(4, 4, 2, new Vector3(2, 0, 0)),
            MultiRectangularRingAnimation.Create(4, 4, 3, new Vector3(6, 0, 0)),
            MultiRectangularRingAnimation.Create(4, 4, 4, new Vector3(10, 0, 0))
        };

        public BurstScene(GraphicsDevice graphics)
            : base(graphics)
        {
            _effect = new BasicEffect(graphics)
            {
                AmbientLightColor = Vector3.One,
                LightingEnabled = true,
                DiffuseColor = Vector3.One
            };
        }

        public override void DrawOther(GameTime gameTime)
        {
            _effect.EnableDefaultLighting();

            _effect.View =
                Matrix.CreateRotationY(MathHelper.ToRadians(45))
                * Matrix.CreateRotationX(MathHelper.ToRadians(45))
                * Matrix.CreateTranslation(0f, 0, -400f);

            _effect.Projection = Matrix.CreateOrthographic(
                Graphics.Viewport.Width / 25f,
                Graphics.Viewport.Height / 25f,
                0.1f,
                500f);

            foreach (var burst in _bursts)
            {
                burst.Draw(Graphics, _effect);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var burst in _bursts)
            {
                burst.Update(gameTime);
                if (burst.Finished)
                {
                    burst.Reset();
                }
            }
        }
    }
}
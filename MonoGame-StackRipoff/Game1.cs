using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame_StackRipoff
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteFont _font;
        private SpriteBatch _spriteBatch;
        private readonly RasterizerState _rasterizerState = new RasterizerState
        {
            CullMode = CullMode.CullClockwiseFace
        };
        private readonly KeyboardEvents _keyboard = new KeyboardEvents();
        private BasicEffect _basicEffect;

        private const int StartingPrismCount = 3;
        private readonly Stack _stack = new Stack(StartingPrismCount);
        private readonly PrismBouncer _bouncer = new PrismBouncer(
            RectangularPrismFactory.MakeStandard(new Vector3(0, StartingPrismCount * RectangularPrismFactory.TileHeight, 0)),
            24);

        private static float _cameraY = -5f;

        private readonly Animator _cameraAnimator = new Animator(
            Easing.CubicOut,
            -3f,
            val => _cameraY = val,
            - RectangularPrismFactory.TileHeight,
            1);

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720
            };

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _keyboard.OnPress(Keys.Space,
                () =>
                {
                    _bouncer.Prism.OverlapWith(_stack.Top, _bouncer.CurrentAxis).Do(
                        perfect =>
                        {
                            _stack.Push(perfect.Landed);
                            _cameraAnimator.Reset(_cameraY);
                        },
                        totalMiss => { },
                        mixed =>
                        {
                            _stack.Push(mixed.Landed);
                            _cameraAnimator.Reset(_cameraY);
                        });
                    _bouncer.Prism = _stack.CreateNextUnboundPrism();
                    _bouncer.Reset();
                    _bouncer.ToggleDirection();
                });

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("Century Gothic");
            _basicEffect = new BasicEffect(GraphicsDevice)
            {
                AmbientLightColor = Vector3.One,
                LightingEnabled = true,
                DiffuseColor = Vector3.One
            };
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            _keyboard.Update(Keyboard.GetState());
            _bouncer.Update(gameTime);
            _cameraAnimator.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.RasterizerState = _rasterizerState;
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _basicEffect.View =
                Matrix.CreateRotationY(MathHelper.ToRadians(45))
                *Matrix.CreateRotationX(MathHelper.ToRadians(45))
                *Matrix.CreateTranslation(0f, _cameraY, -400f);

            _basicEffect.Projection = Matrix.CreateOrthographic(
                GraphicsDevice.Viewport.Width/25f,
                GraphicsDevice.Viewport.Height/25f,
                1f,
                500f);

            _basicEffect.EnableDefaultLighting();

            foreach (var prism in _stack.Prisms)
            {
                drawPrism(prism);
            }
            drawPrism(_bouncer.Prism);

            _spriteBatch.Begin();
            var score = _stack.Score.ToString();
            
            
            _spriteBatch.DrawString(
                _font,
                score,
                new Vector2((GraphicsDevice.Viewport.Width - _font.MeasureString(score).X)/2, 0),
                Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void drawPrism(RectangularPrism prism)
        {
            _basicEffect.World = prism.WorldMatrix;
            _basicEffect.DiffuseColor = prism.Color.ToVector3();
            foreach (var pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, prism.Vertices, 0, prism.Vertices.Length/3);
            }
        }
    }
}
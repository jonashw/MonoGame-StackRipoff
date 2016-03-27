using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame_StackRipoff
{
    public class Game1 : Game
    {
        private readonly MarkerLine _yAxisMarker = new MarkerLine()
        {
            StartPoint = new Vector3(0,-500,0),
            EndPoint = new Vector3(0,500,0)
        };

        private readonly GraphicsDeviceManager _graphics;
        private SpriteFont _font;
        private SpriteFont _debugFont;
        private SpriteBatch _spriteBatch;
        private readonly List<RectangularPrism> _discardedPrisms = new List<RectangularPrism>();
        private readonly List<IRectangularRingAnimation> _bursts = new List<IRectangularRingAnimation>();
        private readonly RasterizerState _rasterizerState = new RasterizerState
        {
            CullMode = CullMode.CullClockwiseFace
        };
        private readonly KeyboardEvents _keyboard = new KeyboardEvents();
        private BasicEffect _basicEffect;
        private BasicEffect _starkWhiteEffect;

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
            _keyboard.OnPress(Keys.Space, placeCurrentPrism);
            base.Initialize();
        }


        private void placeCurrentPrism()
        {
            _bouncer.Prism.OverlapWith(_stack.Top, _bouncer.CurrentAxis).Do(
                perfect =>
                {
                    _stack.Push(perfect.Landed);
                    _cameraAnimator.Reset(_cameraY);
                    _bursts.Add(PlayRegistry.Perfect(perfect));
                },
                totalMiss =>
                {
                    _discardedPrisms.Add(_bouncer.Prism);
                    PlayRegistry.NotPerfect();
                },
                mixed =>
                {
                    _stack.Push(mixed.Landed);
                    _discardedPrisms.Add(mixed.Missed);
                    _cameraAnimator.Reset(_cameraY);
                    PlayRegistry.NotPerfect();
                });
            _bouncer.Prism = _stack.CreateNextUnboundPrism();
            _bouncer.Reset();
            _bouncer.ToggleDirection();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("Century Gothic");
            _debugFont = Content.Load<SpriteFont>("Consolas");
            _basicEffect = new BasicEffect(GraphicsDevice)
            {
                AmbientLightColor = Vector3.One,
                LightingEnabled = true,
                DiffuseColor = Vector3.One
            };
            _starkWhiteEffect = new BasicEffect(GraphicsDevice)
            {
                AmbientLightColor = Vector3.One,
                DiffuseColor = Vector3.One
            };
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            foreach (var p in _discardedPrisms)
            {
                //p.YVelocity -= 0.03f;
                p.Opacity -= 0.03f;
                //p.Position.Y += p.YVelocity;
            }

            _keyboard.Update(Keyboard.GetState());
            _bouncer.Update(gameTime);
            _cameraAnimator.Update(gameTime);

            foreach (var burst in _bursts)
            {
                burst.Update(gameTime);
            }

            var unfinishedBursts = _bursts.Where(b => !b.Finished).ToList();
            _bursts.Clear();
            _bursts.AddRange(unfinishedBursts);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.RasterizerState = _rasterizerState;
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _basicEffect.View =
                Matrix.CreateTranslation(-0f, _cameraY, -0f)
                *Matrix.CreateRotationY(MathHelper.ToRadians(45))
                *Matrix.CreateRotationX(MathHelper.ToRadians(45))
                *Matrix.CreateTranslation(0, 0, -100f);


            _basicEffect.Projection = Matrix.CreateOrthographic(
                GraphicsDevice.Viewport.Width/25f,
                GraphicsDevice.Viewport.Height/25f,
                0.1f,
                500f);

            _basicEffect.EnableDefaultLighting();

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            _bouncer.Prism.Draw(GraphicsDevice, _basicEffect);
            foreach (var prism in _stack.Prisms)
            {
                prism.Draw(GraphicsDevice, _basicEffect);
            }
            foreach (var prism in _discardedPrisms)
            {
                prism.Draw(GraphicsDevice, _basicEffect);
            }
            foreach (var burst in _bursts)
            {
                burst.Draw(GraphicsDevice, _basicEffect);
            }

            //_yAxisMarker.Draw(GraphicsDevice, _starkWhiteEffect);

            _spriteBatch.Begin();
            var score = _stack.Score.ToString(CultureInfo.InvariantCulture);
            
            _spriteBatch.DrawString(
                _font,
                score,
                new Vector2((GraphicsDevice.Viewport.Width - _font.MeasureString(score).X)/2, 0),
                Color.White);

            _spriteBatch.DrawString(_debugFont, string.Format("Camera.Y=" + _cameraY), new Vector2(24,24), Color.White);
            _spriteBatch.DrawString(_debugFont, string.Format("Stack.Top.Y=" + _stack.Top.Position.Y), new Vector2(24,48), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
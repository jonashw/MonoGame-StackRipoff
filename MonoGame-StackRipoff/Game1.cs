using System;
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
        private const int StartingPrismCount = 3;

        private readonly MarkerLine _yAxisMarker = new MarkerLine()
        {
            StartPoint = new Vector3(0,-500,0),
            EndPoint = new Vector3(0,500,0)
        };

        private SpriteDissipator _playButton;
        private SpriteDissipator _logo;
        private readonly EasyTimer _particleTimer = new EasyTimer(TimeSpan.FromSeconds(0.75f));
        private ParticleSystem _particleSystem;
        private readonly GraphicsDeviceManager _graphics;
        private SpriteFont _font;
        private SpriteFont _debugFont;
        private SpriteBatch _spriteBatch;
        private readonly List<RectangularPrism> _discardedPrisms = new List<RectangularPrism>();
        private readonly List<IRectangularRingAnimation> _bursts = new List<IRectangularRingAnimation>();
        private GameState _state = GameState.Welcome;
        private readonly RasterizerState _rasterizerState = new RasterizerState
        {
            CullMode = CullMode.CullClockwiseFace
        };
        private readonly KeyboardEvents _keyboard = new KeyboardEvents();
        private readonly MouseEvents _mouse;
        private BasicEffect _basicEffect;
        private BasicEffect _starkWhiteEffect;

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
            _bouncer.Enabled = false;
            _mouse = new MouseEvents(Window);
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
            _particleSystem = new ParticleSystem(GraphicsDevice, Content);

            var playButtonTexture = Content.Load<Texture2D>("PlayButton");

            _playButton = new SpriteDissipator(
                playButtonTexture,
                GraphicsDevice.Viewport.GetCenter() - new Vector2(playButtonTexture.Width, playButtonTexture.Height)/2f);

            var logoTexture = Content.Load<Texture2D>("Logo");
            _logo = new SpriteDissipator(
                logoTexture,
                GraphicsDevice.Viewport.GetCenter() - new Vector2(logoTexture.Width, logoTexture.Height + 500)/2f);
        }

        protected override void Initialize()
        {
            _keyboard.OnPress(Keys.Space, () =>
            {
                switch (_state)
                {
                    case GameState.Welcome:
                        startPlaying();
                        break;
                    case(GameState.Playing):
                        placeCurrentPrism();
                        break;
                    case GameState.GameOver:
                        backToWelcome();
                        break;
                }
            });
            _keyboard.OnPress(Keys.Enter, () =>
            {
                _zoomedOut = !_zoomedOut;
            });
            _mouse.OnLeftClick((x, y) =>
            {
                if (_state == GameState.Welcome)
                {
                    startPlaying();
                }
            });

            base.Initialize();
        }

        private void startPlaying()
        {
            if (_state != GameState.Welcome)
            {
                return;
            }
            _state = GameState.Starting;
            _playButton.Start();
            _logo.Start();
        }

        private void playing()
        {
            if (_state != GameState.Starting)
            {
                return;
            }
            _bouncer.Prism = _stack.CreateNextUnboundPrism();
            _bouncer.Enabled = true;
            _state = GameState.Playing;
        }

        private void gameOver()
        {
            if (_state != GameState.Playing)
            {
                return;
            }
            _zoomedOut = true;
            _bouncer.Enabled = false;
            _state = GameState.GameOver;
            _cameraAnimator.Reset(_cameraY);
        }

        private void backToWelcome()
        {
            if (_state != GameState.GameOver)
            {
                return;
            }
            _cameraAnimator.Reset(-5f);
            _stack.StartOver(StartingPrismCount);
            _state = GameState.Welcome;
            _zoomedOut = false;
            _playButton.Reset();
            _logo.Reset();
        }

        public enum GameState
        {
            Welcome,
            Starting,
            Playing,
            GameOver
        }

        private void placeCurrentPrism()
        {
            _bouncer.Prism.OverlapWith(_stack.Top, _bouncer.CurrentAxis).Do(
                perfect =>
                {
                    _stack.Push(perfect.Landed);
                    _cameraAnimator.Reset(_cameraY);
                    _bursts.Add(PlayRegistry.Perfect(perfect));
                    _bouncer.Prism = _stack.CreateNextUnboundPrism();
                    _bouncer.Reset();
                    _bouncer.ToggleDirection();
                },
                totalMiss =>
                {
                    _discardedPrisms.Add(_bouncer.Prism);
                    PlayRegistry.NotPerfect();
                    _bouncer.Prism = _stack.CreateNextUnboundPrism();
                    _bouncer.Reset();
                    _bouncer.ToggleDirection();
                    gameOver();
                },
                mixed =>
                {
                    _stack.Push(mixed.Landed);
                    _discardedPrisms.Add(mixed.Missed);
                    _cameraAnimator.Reset(_cameraY);
                    PlayRegistry.NotPerfect();
                    _bouncer.Prism = _stack.CreateNextUnboundPrism();
                    _bouncer.Reset();
                    _bouncer.ToggleDirection();
                });
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            _keyboard.Update(Keyboard.GetState());
            _mouse.Update(gameTime);

            foreach (var p in _discardedPrisms)
            {
                //p.YVelocity -= 0.03f;
                p.Opacity -= 0.03f;
                //p.Position.Y += p.YVelocity;
            }

            if (_state == GameState.Playing || _state == GameState.Welcome || _state == GameState.Starting)
            {
                _playButton.Update(gameTime);
                _logo.Update(gameTime);
                if (_state == GameState.Starting && _playButton.State == SpriteDissipatorState.Finished)
                {
                    playing();
                }
            }
            _bouncer.Update(gameTime);
            _cameraAnimator.Update(gameTime);

            if (_particleTimer.IsFinished(gameTime))
            {
                _particleSystem.SpawnRandom(3);
                _particleTimer.Reset(gameTime);
            }
            _particleSystem.Update(gameTime);

            foreach (var burst in _bursts)
            {
                burst.Update(gameTime);
            }

            var unfinishedBursts = _bursts.Where(b => !b.Finished).ToList();
            _bursts.Clear();
            _bursts.AddRange(unfinishedBursts);

            base.Update(gameTime);
        }

        private bool _zoomedOut = false;

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.RasterizerState = _rasterizerState;
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (_zoomedOut)
            {
                float prismCount = _stack.Prisms.Count();
                var scale = 8f/prismCount;//8 prisms is the maximum amount that can fit vertically in orthographic view.
                _basicEffect.View =
                    Matrix.CreateTranslation(-0f, -prismCount, -0f)
                    *Matrix.CreateRotationY(MathHelper.ToRadians(45))
                    *Matrix.CreateRotationX(MathHelper.ToRadians(45))
                    *Matrix.CreateScale(MathHelper.Min(scale,1))
                    *Matrix.CreateTranslation(0, 0, -1000f);
            }
            else
            {
                _basicEffect.View =
                    Matrix.CreateTranslation(-0f, _cameraY, -0f)
                    *Matrix.CreateRotationY(MathHelper.ToRadians(45))
                    *Matrix.CreateRotationX(MathHelper.ToRadians(45))
                    *Matrix.CreateTranslation(0, 0, -100f);
            }


            _basicEffect.Projection = Matrix.CreateOrthographic(
                GraphicsDevice.Viewport.Width/25f,
                GraphicsDevice.Viewport.Height/25f,
                0.1f,
                2000f);

            _basicEffect.EnableDefaultLighting();

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            var score = _stack.Score.ToString(CultureInfo.InvariantCulture);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            _particleSystem.Draw(_spriteBatch);
            _bouncer.Draw(GraphicsDevice, _basicEffect);
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

            _playButton.Draw(_spriteBatch);
            _logo.Draw(_spriteBatch);

            if (_state == GameState.Playing || _state == GameState.GameOver)
            {
                _spriteBatch.DrawString(
                    _font,
                    score,
                    new Vector2((GraphicsDevice.Viewport.Width - _font.MeasureString(score).X)/2, 0),
                    Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
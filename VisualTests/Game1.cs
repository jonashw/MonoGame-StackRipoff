using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_StackRipoff;
using MonoGame_StackRipoff.MonoGameInscribedTriangles;
using VisualTests.TestScenes;

namespace VisualTests
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private Scene _scene;
        private readonly KeyboardEvents _keyboard = new KeyboardEvents();

        private readonly RasterizerState _rasterizerState = new RasterizerState
        {
            CullMode = CullMode.CullClockwiseFace
        };

        private readonly CircularArray<SceneDescriptor> _sceneDescriptors = new CircularArray<SceneDescriptor>(
            new []
            {
                new SceneDescriptor("Sprite Dissipator", (g,c) => new SpriteDissipatorScene(g,c)), 
                new SceneDescriptor("Particles", (g,c) => new ParticlesScene(g,c)), 
                new SceneDescriptor("Bursts", (g,c) => new BurstScene(g)), 
                new SceneDescriptor("Text Test", (g,c) => new TextScene(g,c)), 
            });

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720
            };
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("Consolas");
            loadScene();
        }

        protected override void Initialize()
        {
            _keyboard.OnPress(Keys.Space, () =>
            {
                if (_scene != null)
                {
                    _sceneDescriptors.Next();
                }
                loadScene();
            });

            _keyboard.OnPress(Keys.Left, () =>
            {
                if (_scene != null)
                {
                    _sceneDescriptors.Prev();
                }
                loadScene();
            });

            _keyboard.OnPress(Keys.Right, () =>
            {
                if (_scene != null)
                {
                    _sceneDescriptors.Next();
                }
                loadScene();
            });
            base.Initialize();
        }

        private void loadScene()
        {
            _scene = _sceneDescriptors.GetCurrent().Create(GraphicsDevice, Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            _keyboard.Update(Keyboard.GetState());

            if (_scene != null)
            {
                _scene.Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.RasterizerState = _rasterizerState;
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            if (_scene != null)
            {
                _scene.DrawOther(gameTime);
                _scene.DrawSprites(_spriteBatch,gameTime);
            }

            var i = 0;
            var itemWidth = GraphicsDevice.Viewport.Width/_sceneDescriptors.Count;
            foreach (var sceneDescriptor in _sceneDescriptors.All)
            {
                var active = sceneDescriptor == _sceneDescriptors.GetCurrent();
                _spriteBatch.FillRectangle(
                    new Rectangle(
                        itemWidth*i,
                        GraphicsDevice.Viewport.Height - 48,
                        itemWidth,
                        48),
                    active ? Color.White : Color.Black);

                var strWidth = _font.MeasureString(sceneDescriptor.Name).X;

                _spriteBatch.DrawString(
                    _font,
                    sceneDescriptor.Name,
                    new Vector2(
                        itemWidth*i + (itemWidth - strWidth)/2f,
                        GraphicsDevice.Viewport.Height - 32),
                    active ? Color.Black : Color.White);
                i++;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace strategyGame.Classes
{
    public class GameWorld : Game
    {

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private static SpriteFont arial;
        private static MouseState mouseState;
        private static KeyboardState keyState;
        private FrameCounter _frameCounter = new FrameCounter();

        public static List<GameObject> gameObjects = new List<GameObject>();
        private static List<GameObject> newGameObjects = new List<GameObject>();
        private static List<GameObject> removeGameObjects = new List<GameObject>();

        private static List<UIElement> UIList = new List<UIElement>();


        private static List<string> debugTexts = new List<string>();

        private Texture2D collisionTexture;
        private Texture2D backgroundImage;

        private static Vector2 screenSize;
        private static Vector2 oldScreenSize;


        public static List<UIElement> UIListProp { get => UIList; set => UIList = value; }
        public static Vector2 ScreenSize { get => screenSize; set => screenSize = value; }
        public static MouseState MouseStateProp { get => mouseState; set => mouseState = value; }
        public static List<string> DebugTexts { get => debugTexts; set => debugTexts = value; }
        public static SpriteFont Arial { get => arial; set => arial = value; }
        public static Vector2 OldScreenSize { get => oldScreenSize; set => oldScreenSize = value; }
        public static KeyboardState KeyStateProp { get => keyState; set => keyState = value; }

        public GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1600;
            _graphics.PreferredBackBufferHeight = 900;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            ScreenSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnResize;
        }

        public void OnResize(Object sender, EventArgs e)
        {
            OldScreenSize = screenSize;
            screenSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            MapHandler.OnResize();
            foreach (Province province in MapHandler.Map)
            {
                province.OnResize();
            }

            foreach (UIElement ui in UIList)
            {
                //Updates the position of UI elements
                ui.UpdatePosition();
            }
        }

        protected override void Initialize()
        {
            arial = Content.Load<SpriteFont>("arial");

            PlayerHandler.CreatePlayers();

            UIHandler.LoadUI();

            MapHandler.LoadContent(Content);
            MapHandler.HightlightProp.LoadContent(Content);
            MapHandler.Build();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            collisionTexture = Content.Load<Texture2D>("CollisionTexture");

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.LoadContent(Content);
            }

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            MouseStateProp = Mouse.GetState();
            KeyStateProp = Keyboard.GetState();

            PlayerHandler.UpdatePlayers(gameTime);
            MapHandler.Update();

            foreach (GameObject gameObject in newGameObjects)
            {
                gameObjects.Add(gameObject);
            }
            newGameObjects.Clear();

            foreach (GameObject gameObject in removeGameObjects)
            {
                gameObjects.Remove(gameObject);
            }
            removeGameObjects.Clear();

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.FrontToBack);

            //FPS
            _frameCounter.Update(gameTime);
            var fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);
            _spriteBatch.DrawString(Arial, fps, new Vector2(0, 16), Color.White,
                0, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
            _spriteBatch.DrawString(GameWorld.Arial, mouseState.Position.ToString(), new Vector2(0, screenSize.Y - 48), Color.White,
                0, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);

            //GameObjects
            foreach (GameObject obj in gameObjects)
            {
                obj.Draw(_spriteBatch);
#if DEBUG
                if (obj is Hightlight)
                {

                } else
                {
                    //DrawCollisionBox(obj);
                }
#endif
            }

            DrawMapBoundary(MapHandler.MapRect);

            //UI
            Vector2 mouseHover = new Vector2(mouseState.Position.X -20, mouseState.Position.Y -30);

            UIHandler.UpdateUI();

            foreach (UIElement ui in UIList)
            {
                _spriteBatch.DrawString(Arial, ui.StaticText + ui.Text, ui.Position, Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.7f);
            }

            //show province name on hover
            //TODO: show different values depending on map mode
            if (MapHandler.HightlightProp.HoverProvince != null)
            {
                Province selected = MapHandler.HightlightProp.HoverProvince;
                string text = String.Format("{0} {1} {2}", selected.Prefix, selected.Name, selected.Suffix);
                _spriteBatch.DrawString(Arial, text, mouseHover, Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.7f);
            }


            //Draw extra debug texts
            for (int i = 0; i < DebugTexts.Count; i++)
            {
                _spriteBatch.DrawString(Arial, DebugTexts[i], new Vector2(0, 160 + i * 24), Color.DarkRed, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
            }
            DebugTexts.Clear();

            _spriteBatch.End();
        }

        private void DrawMapBoundary(Rectangle rect)
        {
            int lineWidth = 5;
            Color color = Color.DarkGray;

            //rect.X = rect.X - (int)ScreenSize.X / 2;
            //rect.Y = rect.Y - (int)ScreenSize.Y / 2;

            Rectangle topLine = new Rectangle(rect.X - lineWidth, rect.Y - lineWidth, rect.Width + lineWidth*2, lineWidth);
            Rectangle bottomLine = new Rectangle(rect.X, rect.Y + rect.Height, rect.Width, lineWidth);
            Rectangle rightLine = new Rectangle(rect.X + rect.Width, rect.Y, lineWidth, rect.Height + lineWidth);
            Rectangle leftLine = new Rectangle(rect.X - lineWidth, rect.Y, lineWidth, rect.Height + lineWidth);

            _spriteBatch.Draw(collisionTexture, topLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 0.1f);
            _spriteBatch.Draw(collisionTexture, bottomLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 0.1f);
            _spriteBatch.Draw(collisionTexture, rightLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 0.1f);
            _spriteBatch.Draw(collisionTexture, leftLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 0.1f);
        }

        public static void Instantiate(GameObject gameObject)
        {
            newGameObjects.Add(gameObject);
        }

        public static void Destroy(GameObject gameobject)
        {
            removeGameObjects.Add(gameobject);
        }
    }
}

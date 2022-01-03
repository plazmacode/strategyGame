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
        private static SpriteBatch _spriteBatch;
        private static SpriteFont arial;
        private static float zoomScale;
        private FrameCounter _frameCounter = new FrameCounter();

        public static List<GameObject> gameObjects = new List<GameObject>();
        private static List<GameObject> newGameObjects = new List<GameObject>();
        private static List<GameObject> removeGameObjects = new List<GameObject>();




        private static List<string> debugTexts = new List<string>();

        private static Texture2D collisionTexture;

        private static Vector2 screenSize;
        private static Vector2 oldScreenSize;
        private static Vector2 cameraPosition;


        public static Vector2 ScreenSize { get => screenSize; set => screenSize = value; }
        public static List<string> DebugTexts { get => debugTexts; set => debugTexts = value; }
        public static SpriteFont Arial { get => arial; set => arial = value; }
        public static Vector2 OldScreenSize { get => oldScreenSize; set => oldScreenSize = value; }
        public static float ZoomScale { get => zoomScale; set => zoomScale = value; }
        public static Vector2 CameraPosition { get => cameraPosition; set => cameraPosition = value; }

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
            //_graphics.SynchronizeWithVerticalRetrace = false; //Unlocks FPS
            //this.IsFixedTimeStep = false;
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

            foreach (UIElement ui in UIHandler.UIListProp)
            {
                //Updates the position of UI elements
                ui.UpdatePosition();
            }
        }

        protected override void Initialize()
        {
            arial = Content.Load<SpriteFont>("arial");
            cameraPosition = Vector2.Zero;
            //PlayerHandler.CreatePlayers();
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

            InputHandler.Update();
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

            //mouse position
            _spriteBatch.DrawString(Arial, InputHandler.MouseStateProp.Position.ToString(), new Vector2(0, screenSize.Y - 48), Color.White,
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
            Vector2 mouseHover = new Vector2(InputHandler.MouseStateProp.Position.X -20, InputHandler.MouseStateProp.Position.Y - 30);

            UIHandler.UpdateUI();
            UIHandler.DrawTexts(_spriteBatch);

            //show province name on hover
            //TODO: Hold ctrl or shift for more info
            //TODO: show different values depending on map mode
            if (MapHandler.HightlightProp.HoverProvince != null)
            {
                Province selected = MapHandler.HightlightProp.HoverProvince;
                string text = String.Format("{0} {1} {2}", selected.Prefix, selected.Name, selected.Suffix);
                _spriteBatch.DrawString(Arial, text, mouseHover, Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.7f);
                DrawRect(new Rectangle((int)mouseHover.X, (int)mouseHover.Y, (int)Arial.MeasureString(text).X+12,24),Color.Gray,0.6f);
            }

            DebugTexts.Add(ZoomScale.ToString());
            GameWorld.DebugTexts.Add("MapHandler.GeneratingMap: " + MapHandler.GeneratingMap.ToString());
            //Draw extra debug texts
            for (int i = 0; i < DebugTexts.Count; i++)
            {
                _spriteBatch.DrawString(Arial, DebugTexts[i], new Vector2(0, 240 + i * 24), Color.DarkRed, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
            }
            DebugTexts.Clear();

            _spriteBatch.End();
        }

        public static void DrawRect(Rectangle rect, Color color, float layer)
        {
            _spriteBatch.Draw(collisionTexture, rect, null, color, 0, Vector2.Zero, SpriteEffects.None, layer);
        }

        private void DrawMapBoundary(Rectangle rect)
        {
            int lineWidth = 5;
            Color color = Color.DarkGray;

            //rect.X = rect.X + (int)CameraPosition.X;
            //rect.Y = rect.Y + (int)CameraPosition.Y;

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

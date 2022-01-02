using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace strategyGame.Classes
{
    public static class PlayerHandler
    {
        private static MouseState oldState;
        private static KeyboardState oldKeyState;
        private static float cameraSpeed;
        private static Vector2 draggingStartPosition;
        private static Vector2 draggingPosition;
        private static Dictionary<string,Player> playerList = new Dictionary<string,Player>();

        public static Dictionary<string, Player> PlayerList { get => playerList; set => playerList = value; }
        public static Vector2 DraggingPosition { get => draggingPosition; set => draggingPosition = value; }

        static PlayerHandler()
        {
            cameraSpeed = 10;
        }

        public static void CreatePlayers()
        {
            //TODO make this more dynamic when creating many players, also add ai parameter
            playerList.Clear();

            Player player1 = new Player(Color.Blue, "player1");
            Player player2 = new Player(Color.Red, "player2");
            Player player3 = new Player(Color.Green, "player3");
            Player player4 = new Player(Color.Yellow, "player4");
            Player player5 = new Player(Color.HotPink, "player5");
            Player player6 = new Player(Color.Brown, "player6");
            Player player7 = new Player(Color.Purple, "player7");
            Player player8 = new Player(Color.Cyan, "player8");
            Player player9 = new Player(Color.Orange, "player9");
            Player player10 = new Player(Color.LightGreen, "player10");

            //"playerName", objectName
            playerList.Add("player1", player1);
            playerList.Add("player2", player2);
            playerList.Add("player3", player3);
            playerList.Add("player4", player4);
            playerList.Add("player5", player5);
            playerList.Add("player6", player6);
            playerList.Add("player7", player7);
            playerList.Add("player8", player8);
            playerList.Add("player9", player9);
            playerList.Add("player10", player10);
        }

        public static void UpdatePlayers(GameTime gameTime)
        {
            foreach (Player player in PlayerList.Values)
            {
                player.Update(gameTime);
            }
            if (oldKeyState.IsKeyUp(Keys.R) && GameWorld.KeyStateProp.IsKeyDown(Keys.R))
            {
                MapHandler.ClearMap();
                MapHandler.Build();
            }

            if (GameWorld.KeyStateProp.IsKeyDown(Keys.Space))
            {
                if (MapHandler.HightlightProp.SelectedProvince != null)
                {
                    MapHandler.HightlightProp.SelectedProvince.SetBonus(100, "town");
                }
            }

            UpdateCamera();

            oldKeyState = GameWorld.KeyStateProp;
            oldState = GameWorld.MouseStateProp;
        }

        public static void UpdateCamera()
        {
            //if (oldState.MiddleButton == ButtonState.Released && GameWorld.MouseStateProp.MiddleButton == ButtonState.Pressed)
            //{
            //    draggingStartPosition = new Vector2(GameWorld.MouseStateProp.X, GameWorld.MouseStateProp.Y);
            //}
            if (GameWorld.MouseStateProp.MiddleButton == ButtonState.Pressed)
            {
                draggingPosition = new Vector2(oldState.X - GameWorld.MouseStateProp.Position.X,
                    oldState.Y - GameWorld.MouseStateProp.Position.Y);
                GameWorld.CameraPosition -= draggingPosition/GameWorld.ZoomScale;
                MapHandler.OnResize();
                foreach (Province province in MapHandler.Map)
                {
                    province.OnResize();
                }
            }

            if (GameWorld.KeyStateProp.IsKeyDown(Keys.Left))
            {
                GameWorld.CameraPosition = new Vector2(GameWorld.CameraPosition.X + cameraSpeed, GameWorld.CameraPosition.Y);
                MapHandler.OnResize();
                foreach (Province province in MapHandler.Map)
                {
                    province.OnResize();
                }
            }
            if (GameWorld.KeyStateProp.IsKeyDown(Keys.Right))
            {
                GameWorld.CameraPosition = new Vector2(GameWorld.CameraPosition.X - cameraSpeed, GameWorld.CameraPosition.Y);
                MapHandler.OnResize();
                foreach (Province province in MapHandler.Map)
                {
                    province.OnResize();
                }
            }
            if (GameWorld.KeyStateProp.IsKeyDown(Keys.Up))
            {
                GameWorld.CameraPosition = new Vector2(GameWorld.CameraPosition.X, GameWorld.CameraPosition.Y + cameraSpeed);
                MapHandler.OnResize();
                foreach (Province province in MapHandler.Map)
                {
                    province.OnResize();
                }
            }
            if (GameWorld.KeyStateProp.IsKeyDown(Keys.Down))
            {
                GameWorld.CameraPosition = new Vector2(GameWorld.CameraPosition.X, GameWorld.CameraPosition.Y - cameraSpeed);
                MapHandler.OnResize();
                foreach (Province province in MapHandler.Map)
                {
                    province.OnResize();
                }
            }
        }
    }
}

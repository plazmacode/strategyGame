using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace strategyGame.Classes
{
    public static class PlayerHandler
    {
        private static KeyboardState oldKeyState;
        private static Dictionary<string,Player> playerList = new Dictionary<string,Player>();

        public static Dictionary<string, Player> PlayerList { get => playerList; set => playerList = value; }

        public static void CreatePlayers()
        {
            //TODO make this more dynamic when creating many players, also add ai parameter
            Player player1 = new Player(Color.Blue, "player1", 0, 0);
            Player player2 = new Player(Color.Red, "player2", MapHandler.Map.GetLength(0) - 1, MapHandler.Map.GetLength(1) - 1);
            Player player3 = new Player(Color.Green, "player3", 0, MapHandler.Map.GetLength(1) - 1);
            Player player4 = new Player(Color.Yellow, "player4", MapHandler.Map.GetLength(0) - 1, 0);
            Player player5 = new Player(Color.HotPink, "player5", MapHandler.Map.GetLength(0) /2, MapHandler.Map.GetLength(1) / 2);

            //"playerName", objectName
            playerList.Add("player1", player1);
            playerList.Add("player2", player2);
            playerList.Add("player3", player3);
            playerList.Add("player4", player4);
            playerList.Add("player5", player5);
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
            oldKeyState = GameWorld.KeyStateProp;
        }
    }
}

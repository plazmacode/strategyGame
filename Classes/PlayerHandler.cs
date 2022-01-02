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
            Player player1 = new Player(Color.Blue, "player1");
            Player player2 = new Player(Color.Red, "player2");
            Player player3 = new Player(Color.Green, "player3");
            Player player4 = new Player(Color.Yellow, "player4");

            //"playerName", objectName
            playerList.Add("player1", player1);
            playerList.Add("player2", player2);
            playerList.Add("player3", player3);
            playerList.Add("player4", player4);
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
                    MapHandler.HightlightProp.SelectedProvince.GetBonus();
                }
            }
            oldKeyState = GameWorld.KeyStateProp;
        }
    }
}

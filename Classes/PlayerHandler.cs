using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace strategyGame.Classes
{
    public static class PlayerHandler
    {
        private static Dictionary<string,Player> playerList = new Dictionary<string,Player>();

        public static Dictionary<string, Player> PlayerList { get => playerList; set => playerList = value; }

        public static void CreatePlayers()
        {
            Player player1 = new Player(Color.Blue, "player1");
            Player player2 = new Player(Color.Red, "player2");

            player2.SpreadTime = 300;

            playerList.Add("player1",player1);
            playerList.Add("player2",player2);
        }

        public static void UpdatePlayers(GameTime gameTime) {
            foreach (Player player in PlayerList.Values)
            {
                player.Update(gameTime);
            }
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace strategyGame.Classes
{
    public static class PlayerHandler
    {
        private static Dictionary<string,Player> playerList = new Dictionary<string,Player>();

        public static Dictionary<string, Player> PlayerList { get => playerList; set => playerList = value; }

        static PlayerHandler()
        {

        }

        public static void CreatePlayers()
        {
            //TODO make this more dynamic when creating many players, also add ai parameter
            playerList.Clear();
            UIHandler.PlayerTextList.Clear();

            Player player1 = new Player(Color.Blue, "France");
            Player player2 = new Player(Color.Red, "England");
            Player player3 = new Player(Color.Green, "Russia");
            Player player4 = new Player(Color.Yellow, "Spain");
            Player player5 = new Player(Color.HotPink, "Denmark");
            Player player6 = new Player(Color.CadetBlue, "Germany");
            Player player7 = new Player(Color.Purple, "Byzantium");
            Player player8 = new Player(Color.Cyan, "United States");
            Player player9 = new Player(Color.Orange, "Netherlands");
            Player player10 = new Player(Color.LightGreen, "Ottomans");

            //"playerName", objectName
            playerList.Add("player1", player1);
            playerList.Add("player2", player2);
            playerList.Add("player3", player3);
            playerList.Add("player4", player4);
            //playerList.Add("player5", player5);
            //playerList.Add("player6", player6);
            //playerList.Add("player7", player7);
            //playerList.Add("player8", player8);
            //playerList.Add("player9", player9);
            //playerList.Add("player10", player10);

            foreach (Player player in playerList.Values)
            {
                UIHandler.PlayerTextList.Add(new UIElement(player.Name, player.Name, player.Name, "playerText", 0, 0));
            }
        }

        public static void UpdatePlayers(GameTime gameTime)
        {
            foreach (Player player in PlayerList.Values)
            {
                player.Update(gameTime);
            }
        }

        public static void UpdateCamera()
        {
            
        }
    }
}

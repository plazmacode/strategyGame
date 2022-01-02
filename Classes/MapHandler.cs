using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace strategyGame.Classes
{
    public static class MapHandler
    {
        private static Rectangle mapRect;
        private static Province[,] map; //10x10 map
        private static Texture2D[] sprites;
        private static int provinceSize;
        private static float provinceScale;
        private static Vector2 offset;
        private static Vector2 oldOffset;
        private static bool mapActive;
        private static bool generatingMap;

        private static List<string> genericPrefixList = new List<string>();
        private static List<string> genericNameList = new List<string>();
        private static List<string> genericInfixList = new List<string>();
        private static List<string> prefixList = new List<string>();
        private static List<string> realNameList = new List<string>();
        private static List<string> suffixList = new List<string>();

        private static Hightlight hightlight = new Hightlight();

        public static Vector2 Offset { get => offset; set => offset = value; }
        internal static Province[,] Map { get => map; set => map = value; }
        public static Vector2 OldOffset { get => oldOffset; set => oldOffset = value; } //Unused???
        public static Rectangle MapRect { get => mapRect; set => mapRect = value; }
        public static Hightlight HightlightProp { get => hightlight; set => hightlight = value; }
        /// <summary>
        /// spriteSize * provinceScale * ZoomScale
        /// </summary>
        public static int ProvinceSize { get => provinceSize; set => provinceSize = value; }
        public static List<string> GenericNameList { get => genericNameList; set => genericNameList = value; }
        public static List<string> PrefixList { get => prefixList; set => prefixList = value; }
        public static bool MapActive { get => mapActive; set => mapActive = value; }
        public static List<string> SuffixList { get => suffixList; set => suffixList = value; }
        public static float ProvinceScale { get => provinceScale; set => provinceScale = value; }
        public static Texture2D[] Sprites { get => sprites; set => sprites = value; }
        public static List<string> RealNameList { get => realNameList; set => realNameList = value; }
        public static List<string> GenericPrefixList { get => genericPrefixList; set => genericPrefixList = value; }
        public static List<string> GenericInfixList { get => genericInfixList; set => genericInfixList = value; }
        public static bool GeneratingMap { get => generatingMap; set => generatingMap = value; }

        static MapHandler()
        {

            Map = new Province[100, 80]; //Exceed 160x100 and it might start lagging, depending on province texture
            MapActive = false; //Used for highlight
            GameWorld.ZoomScale = 1f;
            provinceScale = 1f;
            GameWorld.Instantiate(HightlightProp);
            LoadNames();
        }

        private static void LoadNames()
        {
            genericPrefixList.Add("East");
            genericPrefixList.Add("West");
            genericPrefixList.Add("Mount");
            genericPrefixList.Add("Great");
            genericPrefixList.Add("High");

            PrefixList.Add("New");
            PrefixList.Add("Old");
            PrefixList.Add("Grand");

            //generic names also a kind of prefix
            GenericNameList.Add("Shef");
            GenericNameList.Add("Clif");
            GenericNameList.Add("Bleak");
            GenericNameList.Add("Chester");
            GenericNameList.Add("Inver");
            GenericNameList.Add("Cape");
            GenericNameList.Add("York");
            GenericNameList.Add("Card");
            GenericNameList.Add("Swan");
            GenericNameList.Add("Herm");
            GenericNameList.Add("Eas");
            GenericNameList.Add("Wel");
            GenericNameList.Add("Tel");
            GenericNameList.Add("Lich");
            GenericNameList.Add("Can");
            GenericNameList.Add("Mil");
            GenericNameList.Add("New");
            GenericNameList.Add("Old");
            GenericNameList.Add("Snare");
            GenericNameList.Add("Whitting");
            GenericNameList.Add("Stoke");
            GenericNameList.Add("Elm");

            //real names
            RealNameList.Add("New York");
            RealNameList.Add("Los Angeles");
            RealNameList.Add("Shenzen");
            RealNameList.Add("Lahore");
            RealNameList.Add("York");
            RealNameList.Add("Amsterdam");
            RealNameList.Add("Copenhagen");
            RealNameList.Add("Shanghai");
            RealNameList.Add("Sydney");
            RealNameList.Add("Melbourne");
            RealNameList.Add("Sao Paulo");
            RealNameList.Add("Mexico");
            RealNameList.Add("Mumbai");
            RealNameList.Add("Dhaka");
            RealNameList.Add("Osaka");
            RealNameList.Add("Chongqing");
            RealNameList.Add("Berlin");
            RealNameList.Add("London");
            RealNameList.Add("Istanbul");
            RealNameList.Add("Tokyo");
            RealNameList.Add("Beijing");
            RealNameList.Add("Delhi");
            RealNameList.Add("Cairo");
            RealNameList.Add("Kinshasa");
            RealNameList.Add("Chicago");
            RealNameList.Add("Bogota");
            RealNameList.Add("Quito");
            RealNameList.Add("Toronto");

            suffixList.Add("Canyon");
            suffixList.Add("Valley");
            suffixList.Add("Hill");
            suffixList.Add("Plain");
            suffixList.Add("Plain");

            GenericInfixList.Add("field");
            GenericInfixList.Add("hull");
            GenericInfixList.Add("berg");
            GenericInfixList.Add("hill");
            GenericInfixList.Add("hall");
            GenericInfixList.Add("ford");
            GenericInfixList.Add("ham");
            GenericInfixList.Add("minster");
            GenericInfixList.Add("shaw");
            GenericInfixList.Add("ton");
            GenericInfixList.Add("ing");
            GenericInfixList.Add("dale");
            GenericInfixList.Add("thorpe");
            GenericInfixList.Add("thwaite");
            GenericInfixList.Add("nell");
            GenericInfixList.Add("by");
            GenericInfixList.Add("stone");
            GenericInfixList.Add("ock");
        }

        public static void LoadContent(ContentManager content)
        {
            Sprites = new Texture2D[4];
            //make sure you have the textures ad set the sprites size in the static constructor
            for (int i = 0; i < sprites.Length; i++)
            {
                //province +: small, mini, micro
                //For extra small use collisionTexture
                Sprites[i] = content.Load<Texture2D>("provinceSmall" + i);
            }
            hightlight.OnResize(); //Fixes size of highlight based on sprite size chosen
            ProvinceSize = (int)(Sprites[0].Width * provinceScale * GameWorld.ZoomScale); //assuming all provinces are squares and have same size as first province texture
        }

        public static void OnResize()
        {
            ProvinceSize = (int)(Sprites[0].Width * provinceScale * GameWorld.ZoomScale); //assuming all provinces are squares and have same size as first province texture
            oldOffset = offset;
            offset = new Vector2(GameWorld.ScreenSize.X / 2 - Map.GetLength(0) * provinceSize / 2,
                (GameWorld.ScreenSize.Y / 2 - Map.GetLength(1) * provinceSize / 2)) + (GameWorld.CameraPosition * GameWorld.ZoomScale);
            MapRect = new Rectangle((int)offset.X, (int)offset.Y, map.GetLength(0) * provinceSize, map.GetLength(1) * provinceSize);
        }

        public static void ClearMap()
        {
            //GameWorld.CameraPosition = Vector2.Zero;
            foreach (GameObject obj in GameWorld.gameObjects)
            {
                if (obj is Province)
                {
                    GameWorld.Destroy(obj);
                }
            }
        }

        public static void Build()
        {
            generatingMap = true;
            offset = new Vector2(GameWorld.ScreenSize.X / 2 - Map.GetLength(0) * provinceSize / 2,
                (GameWorld.ScreenSize.Y / 2 - Map.GetLength(1) * provinceSize / 2));
            offset = offset + (GameWorld.CameraPosition * GameWorld.ZoomScale);
            MapRect = new Rectangle((int)offset.X, (int)offset.Y, map.GetLength(0) * provinceSize, map.GetLength(1) * provinceSize);

            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    int x =  i;
                    int y = j;
                    Province province = new Province(x, y, Sprites[0]); //x, y sends the position in the map[] array
                    province.Position = new Vector2(provinceSize * x, provinceSize * y) + Offset; //x, y sets the correct position
                    province.ProvinceRect = new Rectangle((int)province.Position.X, (int)province.Position.Y, provinceSize, provinceSize);
                    Map[i, j] = province;
                    GameWorld.Instantiate(province);
                }
            }
            PlayerHandler.CreatePlayers();

            foreach (Player player in PlayerHandler.PlayerList.Values)
            {
                if (player.X != -1)
                {
                    map[player.X, player.Y].Owner = player.Name;
                    map[player.X, player.Y].SetBonus(0, "capital");
                }
            }
        }

        public static void Update()
        {
            if (MapHandler.MapRect.Contains(GameWorld.MouseStateProp.Position))
            {
                MapActive = true;
            }
            else
            {
                MapActive = false;
                MapHandler.HightlightProp.HoverProvince = null;
                MapHandler.HightlightProp.Position = new Vector2(-100, -100);
            }
        }
    }
}

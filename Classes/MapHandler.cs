using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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

        private static List<string> prefixList = new List<string>();
        private static List<string> nameList = new List<string>();
        private static List<string> suffixList = new List<string>();

        private static Hightlight hightlight = new Hightlight();

        public static Vector2 Offset { get => offset; set => offset = value; }
        internal static Province[,] Map { get => map; set => map = value; }
        public static Vector2 OldOffset { get => oldOffset; set => oldOffset = value; }
        public static Rectangle MapRect { get => mapRect; set => mapRect = value; }
        public static Hightlight HightlightProp { get => hightlight; set => hightlight = value; }
        public static int ProvinceSize { get => provinceSize; set => provinceSize = value; }
        public static List<string> NameList { get => nameList; set => nameList = value; }
        public static List<string> PrefixList { get => prefixList; set => prefixList = value; }
        public static bool MapActive { get => mapActive; set => mapActive = value; }
        public static List<string> SuffixList { get => suffixList; set => suffixList = value; }
        public static float ProvinceScale { get => provinceScale; set => provinceScale = value; }
        public static Texture2D[] Sprites { get => sprites; set => sprites = value; }

        static MapHandler()
        {
            MapActive = false;
            Map = new Province[80, 60]; //Exceed 160x100 and it might start lagging, depending on province texture
            Sprites = new Texture2D[2];
            provinceScale = 1f;
            GameWorld.Instantiate(HightlightProp);
            LoadNames();
        }

        private static void LoadNames()
        {
            PrefixList.Add("New");
            PrefixList.Add("Old");
            PrefixList.Add("Grand");

            //TODO add generics and place real world names in different list
            NameList.Add("York");
            NameList.Add("Amsterdam");
            NameList.Add("Copenhagen");
            NameList.Add("Berlin");
            NameList.Add("London");
            NameList.Add("Istanbul");
            NameList.Add("Tokyo");
            NameList.Add("Beijing");
            NameList.Add("Delhi");
            NameList.Add("Cairo");
            NameList.Add("Kinshasa");
            NameList.Add("Chicago");
            NameList.Add("Bogota");
            NameList.Add("Quito");
            NameList.Add("Toronto");

            suffixList.Add("Canyon");
            suffixList.Add("Valley");
            suffixList.Add("Hill");
            suffixList.Add("Plain");
        }

        public static void LoadContent(ContentManager content)
        {
            //make sure you have the textures ad set the sprites size in the static constructor
            for (int i = 0; i < 2; i++)
            {
                //province +: small, mini, micro
                //For extra small use collisionTexture
                Sprites[i] = content.Load<Texture2D>("provinceSmall" + i);
            }
            ProvinceSize = (int)(Sprites[0].Width * provinceScale); //assuming all provinces are squares and have same size as first province texture
        }

        public static void OnResize()
        {
            oldOffset = offset;
            offset = new Vector2(GameWorld.ScreenSize.X / 2 - Map.GetLength(0) * (int)(provinceSize * provinceScale) / 2,
                (GameWorld.ScreenSize.Y / 2 - Map.GetLength(1) * (int)(provinceSize * provinceScale) / 2));
            MapRect = new Rectangle((int)offset.X, (int)offset.Y, map.GetLength(0) * (int)(provinceSize * provinceScale), map.GetLength(1) * (int)(provinceSize * provinceScale));

        }

        public static void ClearMap()
        {
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
            offset = new Vector2(GameWorld.ScreenSize.X / 2 - Map.GetLength(0) * (int)(provinceSize * provinceScale) / 2,
                (GameWorld.ScreenSize.Y / 2 - Map.GetLength(1) * (int)(provinceSize * provinceScale) / 2));
            MapRect = new Rectangle((int)offset.X, (int)offset.Y, map.GetLength(0) * (int)(provinceSize * provinceScale), map.GetLength(1) * (int)(provinceSize * provinceScale));

            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    int x =  i;
                    int y = j;
                    Province province = new Province(x, y, Sprites[0]); //x, y sends the position in the map[] array
                    province.Position = new Vector2((int)(provinceScale * provinceSize) * x, (int)(provinceScale * provinceSize) * y) + Offset; //x, y sets the correct position
                    province.ProvinceRect = new Rectangle((int)province.Position.X, (int)province.Position.Y, (int)(ProvinceSize * provinceScale), (int)(ProvinceSize * provinceScale));
                    Map[i, j] = province;
                    GameWorld.Instantiate(province);
                }
            }
            map[0, 0].Owner = "player1";
            map[0, 0].SetCapital();
            map[map.GetLength(0) - 1, map.GetLength(1) - 1].Owner = "player2";
            map[map.GetLength(0) - 1, map.GetLength(1) - 1].SetCapital();
            map[0, map.GetLength(1) - 1].Owner = "player3";
            map[0, map.GetLength(1) - 1].SetCapital();
            map[map.GetLength(0) -1, 0].Owner = "player4";
            map[map.GetLength(0) - 1, 0].SetCapital();
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

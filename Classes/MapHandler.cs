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
        private static Vector2 offset;
        private static Vector2 oldOffset;
        private static bool mapActive;
        private static List<string> nameList = new List<string>();

        private static Hightlight hightlight = new Hightlight();

        public static Vector2 Offset { get => offset; set => offset = value; }
        internal static Province[,] Map { get => map; set => map = value; }
        public static Vector2 OldOffset { get => oldOffset; set => oldOffset = value; }
        public static Rectangle MapRect { get => mapRect; set => mapRect = value; }
        public static Hightlight HightlightProp { get => hightlight; set => hightlight = value; }
        public static int ProvinceSize { get => provinceSize; set => provinceSize = value; }
        public static List<string> NameList { get => nameList; set => nameList = value; }
        public static bool MapActive { get => mapActive; set => mapActive = value; }

        static MapHandler()
        {
            MapActive = false;
            Map = new Province[32, 24]; //BIG MAP, BIG LAG
            sprites = new Texture2D[1];
            GameWorld.Instantiate(HightlightProp);
            LoadNames();
        }

        private static void LoadNames()
        {
            NameList.Add("testName1");
            NameList.Add("testName2");
            NameList.Add("testName3");
            NameList.Add("testName4");
            NameList.Add("testName5");
            NameList.Add("testName6");
            NameList.Add("testName7");
            NameList.Add("testName8");
            NameList.Add("testName9");
            NameList.Add("testName10");
        }

        public static void LoadContent(ContentManager content)
        {
            for (int i = 0; i < 1; i++)
            {
                sprites[i] = content.Load<Texture2D>("province");
            }
            ProvinceSize = sprites[0].Width; //assuming all provinces are squares and have same size as first province texture
        }

        public static void OnResize()
        {
            oldOffset = offset;
            offset = new Vector2(GameWorld.ScreenSize.X / 2 - Map.GetLength(0) * provinceSize / 2, (GameWorld.ScreenSize.Y / 2 - Map.GetLength(1) * provinceSize / 2));
            MapRect = new Rectangle((int)offset.X, (int)offset.Y, map.GetLength(0) * provinceSize, map.GetLength(1) * provinceSize);

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
            offset = new Vector2(GameWorld.ScreenSize.X / 2 - Map.GetLength(0) * provinceSize / 2, (GameWorld.ScreenSize.Y / 2 - Map.GetLength(1) * provinceSize / 2));
            MapRect = new Rectangle((int)offset.X, (int)offset.Y, map.GetLength(0) * provinceSize, map.GetLength(1) * provinceSize);

            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    int x =  i;
                    int y = j;
                    Province province = new Province(x, y, sprites[0]); //x, y sends the position in the map[] array
                    province.Position = new Vector2(provinceSize * x, provinceSize * y) + Offset; //x, y sets the correct position
                    province.ProvinceRect = new Rectangle((int)province.Position.X, (int)province.Position.Y, ProvinceSize, ProvinceSize);
                    Map[i, j] = province;
                    GameWorld.Instantiate(province);
                }
            }
            map[0, 0].Owner = "player1";
            map[0, 0].SetCapital();
            map[map.GetLength(0) - 1, map.GetLength(1) - 1].Owner = "player2";
            map[map.GetLength(0) - 1, map.GetLength(1) - 1].SetCapital();
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

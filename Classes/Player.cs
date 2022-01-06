using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace strategyGame.Classes
{
    /// <summary>
    /// This class contains the functionality of a player. For more, check the PlayerHandler
    /// </summary>
    public class Player
    {
        private Random random;
        private Color color;
        private string name;
        private float spreadTime;
        private int nextSpread;
        private int x;
        private int y;

        public string Name { get => name; set => name = value; }
        public Color Color { get => color; set => color = value; }
        /// <summary>
        /// Millisecond delay between the spread of land when generating the map
        /// <para>can be used to give some players faster spread</para>
        /// <para>if many players are packed together, one of them could spread faster and therefore take more land</para>
        /// </summary>
        public float SpreadTime { get => spreadTime; set => spreadTime = value; }
        public int NextSpread { get => nextSpread; set => nextSpread = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }


        /// <summary>
        /// Creates a player with a color, name and random position
        /// </summary>
        /// <param name="color">The color on the map</param>
        /// <param name="name">The name of the player</param>
        public Player(Color color, string name)
        {
            //x and y is set somewhere else
            RandomPosition();
            this.Name = name;
            this.Color = color;
            SpreadTime = 40;
        }
        /// <summary>
        /// Creates a player with a color, name and a position on the map
        /// </summary>
        /// <param name="color">The color on the map</param>
        /// <param name="name">The name of the player</param>
        /// <param name="x">The X coord on the MapHandler.Map array</param>
        /// <param name="y">The Y coord on the MapHandler.Map array</param>
        public Player(Color color, string name, int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.Name = name;
            this.Color = color;
            SpreadTime = 1; //lower value = faster map generation but more lag
        }

        public void Update(GameTime gameTime)
        {
            //GameWorld.DebugTexts.Add("Name: " + name);
            //GameWorld.DebugTexts.Add("nextSpread: " + NextSpread.ToString());
            //GameWorld.DebugTexts.Add("Seconds: " + gameTime.TotalGameTime.TotalSeconds.ToString());
            if (MapHandler.GeneratingMap)
            {
                Spread(gameTime);
            }
            
        }

        private void RandomPosition()
        {
            random = new Random();
            this.x = random.Next(0, MapHandler.Map.GetLength(0));
            this.y = random.Next(0, MapHandler.Map.GetLength(1));

            //Check if the random position is already owned by someone else
            if (MapHandler.Map[x,y].Owner != null)
            {
                RandomPosition();
            }
        }

        private void Spread(GameTime gameTime)
        {
            //When every province is looped through, check if they can spread to another province
            //if bool CanSpread = false on every province set generating to false
            if (gameTime.TotalGameTime.TotalMilliseconds >= this.nextSpread)
            {
                this.NextSpread = (int)gameTime.TotalGameTime.TotalMilliseconds + (int)this.SpreadTime;
                int totalCanSpreadFalse = 0;
                foreach (Province province in MapHandler.Map)
                {
                    //Checks if the province has an owner etc.
                    if (province.Owner == this.name && province.ControlledSince < gameTime.TotalGameTime.TotalSeconds && province.BonusDistance > 0)
                    {
                        //Check if near-by province is not outside of bounds and then spread to it
                        province.CanSpread = false;
                        //When all province.canSpread = false, then MapHandler.GeneratingMap = false
                        //LEFT
                        if (province.ArrayPosition.X > 0)
                        {
                            Province spread1 = MapHandler.Map[(int)province.ArrayPosition.X - 1, (int)province.ArrayPosition.Y];

                            SpreadNext(spread1);
                        }
                        //RIGHT
                        if (province.ArrayPosition.X < MapHandler.Map.GetLength(0) - 1)
                        {
                            Province spread1 = MapHandler.Map[(int)province.ArrayPosition.X + 1, (int)province.ArrayPosition.Y];
                            SpreadNext(spread1);
                        }
                        //UP
                        if (province.ArrayPosition.Y > 0)
                        {
                            Province spread1 = MapHandler.Map[(int)province.ArrayPosition.X, (int)province.ArrayPosition.Y - 1];
                            SpreadNext(spread1);
                        }
                        //DOWN
                        if (province.ArrayPosition.Y < MapHandler.Map.GetLength(1) - 1)
                        {
                            Province spread1 = MapHandler.Map[(int)province.ArrayPosition.X, (int)province.ArrayPosition.Y + 1];
                            SpreadNext(spread1);
                        }

                        void SpreadNext(Province spread1)
                        {
                            //New provinces have a value: controlledSince, to prevent the map from being instantly controlled
                            if (spread1.Owner == null)
                            {
                                province.CanSpread = true; //The province that started the spread is set to true
                                spread1.BonusDistance += province.BonusDistance - 1;
                                spread1.Color = this.color;
                                spread1.Owner = this.name;
                                spread1.ControlledSince = gameTime.TotalGameTime.TotalSeconds;
                            }
                            //Take other players provinces
                            //Doesn't work properly yet
                            //if (spread1.Owner != this.name)
                            //{
                            //    spread1.BonusDistance = 1;
                            //    spread1.Color = this.color;
                            //    spread1.Owner = this.name;
                            //    spread1.ControlledSince = gameTime.TotalGameTime.TotalSeconds;
                            //}
                        }
                    }
                    if (!province.CanSpread)
                    {
                        totalCanSpreadFalse++;
                    }
                }
                if (totalCanSpreadFalse >= MapHandler.Map.Length)
                {
                    MapHandler.GeneratingMap = false;
                }

            }
        }
    }
}

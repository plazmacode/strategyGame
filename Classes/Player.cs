using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace strategyGame.Classes
{
    public class Player
    {
        private Color color;
        private string name;
        private float spreadTime;
        private int nextSpread;

        public Player(Color color, string name)
        {
            this.Name = name;
            this.Color = color;
            SpreadTime = 40;
        }

        public string Name { get => name; set => name = value; }
        public Color Color { get => color; set => color = value; }
        public float SpreadTime { get => spreadTime; set => spreadTime = value; }
        public int NextSpread { get => nextSpread; set => nextSpread = value; }

        public  void LoadContent(ContentManager content)
        {

        }

        public void Update(GameTime gameTime)
        {
            //GameWorld.DebugTexts.Add("Name: " + name);
            //GameWorld.DebugTexts.Add("nextSpread: " + NextSpread.ToString());
            //GameWorld.DebugTexts.Add("Seconds: " + gameTime.TotalGameTime.TotalSeconds.ToString());

            //Spread
            //TODO: "optimize"
            if (gameTime.TotalGameTime.TotalMilliseconds >= this.nextSpread)
            {
                this.NextSpread = (int)gameTime.TotalGameTime.TotalMilliseconds + (int)this.SpreadTime;
                foreach (Province province in MapHandler.Map)
                {
                    if (province.Owner == this.name && province.ControlledSince < gameTime.TotalGameTime.TotalSeconds && province.BonusDistance > 0)
                    {
                        //Check if near-by province is not outside of bounds

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
                            Province spread1 = MapHandler.Map[(int)province.ArrayPosition.X , (int)province.ArrayPosition.Y + 1];
                            SpreadNext(spread1);
                        }

                        void SpreadNext(Province spread1)
                        {
                            //New provinces have a value: controlledSince, to prevent the map from being instantly controlled

                            if (spread1.Owner == null)
                            {
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
                }
            }
        }
    }
}

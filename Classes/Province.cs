using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace strategyGame.Classes
{
    class Province : GameObject
    {
        private double controlledSince;
        private Rectangle provinceRect;
        private Vector2 arrayPosition;
        private string name;
        private string prefix;
        private string suffix;
        Random random;
        private string owner;
        private Color color;
        private bool bonus;
        private int bonusDistance;

        private List<Building> buildingList = new List<Building>();

        public Rectangle ProvinceRect { get => provinceRect; set => provinceRect = value; }
        public string Name { get => name; set => name = value; }
        public string Owner { get => owner; set => owner = value; }
        public Vector2 ArrayPosition { get => arrayPosition; set => arrayPosition = value; }
        public Color Color { get => color; set => color = value; }
        public double ControlledSince { get => controlledSince; set => controlledSince = value; }
        public bool Bonus { get => bonus; set => bonus = value; }
        public int BonusDistance { get => bonusDistance; set => bonusDistance = value; }
        public string Prefix { get => prefix; set => prefix = value; }
        public string Suffix { get => suffix; set => suffix = value; }

        public Province(int x, int y, Texture2D sprite)
        {
            ArrayPosition = new Vector2(x, y);
            random = new Random();
            GenerateProvince();
            this.sprite = sprite;
            scale = MapHandler.ProvinceScale;
            layerDepth = 0.2f;
            Color = Color.White;
            controlledSince = -1; //-1 means it is unowned or capital
        }


        public override void LoadContent(ContentManager content)
        {
            
        }

        public override void OnCollision(GameObject other)
        {

        }

        public void OnResize()
        {
            Position = Position - MapHandler.OldOffset + MapHandler.Offset;
            ProvinceRect = new Rectangle((int)Position.X, (int)Position.Y, (int)(MapHandler.ProvinceSize * MapHandler.ProvinceScale), (int)(MapHandler.ProvinceSize * MapHandler.ProvinceScale));
        }

        public override void Update(GameTime gameTime)
        {
            CheckHighlight(); //Runs highlight and click code
            //TODO: production
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, Position, null, Color, rotation, origin, MapHandler.ProvinceScale, SpriteEffects.None, layerDepth);
        }

        private void CheckHighlight()
        {
            foreach (Player player in PlayerHandler.PlayerList.Values)
            {
                if (this.Owner == player.Name)
                {
                    this.Color = player.Color;
                }
            }

            //TODO: optimize this code to run from MapHandler

            if (MapHandler.MapActive)
            {
                if (provinceRect.Contains(GameWorld.MouseStateProp.Position))
                {
                    MapHandler.HightlightProp.Position = position;
                    MapHandler.HightlightProp.HoverProvince = this;
                    if (GameWorld.MouseStateProp.LeftButton == ButtonState.Pressed)
                    {
                        OnClick();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnClick()
        {
            MapHandler.HightlightProp.SelectedProvince = this;
        }

        /// <summary>
        /// Gives the province a name and resources
        /// </summary>
        private void GenerateProvince()
        {
            //chance of province giving more land in beginning
            int bonusChance = random.Next(0, 15);

            if (bonusChance == 0)
            {
                this.BonusDistance = 3;
                this.bonus = true;
            }
            prefix = GenerateName("prefix");
            name = GenerateName("name");
            suffix = GenerateName("suffix");
        }

        /// <summary>
        /// Returns string of place names of type: prefix, name and suffix
        /// </summary>
        /// <param name="type">prefix, name, suffix</param>
        /// <returns></returns>
        private string GenerateName(string type)
        {
            int randomName = random.Next(0, MapHandler.NameList.Count);
            int randomPrefix = random.Next(0, MapHandler.PrefixList.Count);
            int randomSuffix = random.Next(0, MapHandler.SuffixList.Count);
            if (type == "name")
            {
                return MapHandler.NameList[randomName];
            }
            else if (type == "prefix")
            {
                //20% chance of prefix
                int prefixChance = random.Next(0, 5);
                if (prefixChance == 0)
                {
                    return MapHandler.PrefixList[randomPrefix];
                } else

                return "";
            }
            else if (type == "suffix")
            {
                //50% chance of suffix
                int prefixChance = random.Next(0, 1);
                if (prefixChance == 0)
                {
                    return MapHandler.SuffixList[randomSuffix];
                }
                else
                    return "";
            }
            else
            {
                return "wrong type";
            }

        }

        /// <summary>
        /// Changes the province to a capital province
        /// </summary>
        public void SetCapital()
        {
            this.suffix = "City";
            this.bonus = true;
            this.bonusDistance = 3;
        }

        /// <summary>
        /// Changes the province to a bonus province
        /// <br>Suffix = Town</br>
        /// <br>Bonus = True</br>
        /// <br>BonusDistnace = 3</br>
        /// </summary>
        public void GetBonus()
        {
            this.suffix = "Town";
            this.bonus = true;
            this.bonusDistance = 3;
        }
    }
}

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
        private Texture2D[] sprites;
        private double controlledSince;
        private Rectangle provinceRect;
        private Vector2 arrayPosition;
        private string name;
        private string prefix;
        private string suffix;
        Random random;
        private string owner;
        private bool bonus; //TODO: rename bonus variables
        private int bonusDistance;
        private bool canSpread;
        private bool isCapital;

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
        public bool CanSpread { get => canSpread; set => canSpread = value; }
        public bool IsCapital { get => isCapital; set => isCapital = value; }

        public Province(int x, int y, Texture2D sprite)
        {
            ArrayPosition = new Vector2(x, y);
            random = new Random();
            this.sprite = sprite;
            GenerateProvince();
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
        /// <summary>
        /// Updates position and provinceRect
        /// </summary>
        public void OnResize()
        {
            //TODO: Call this function less
            //Moving the map causes lag, as each frame the game has to update every province position
            //Potential fix would be to keep the position,
            //but only change the Draw() to position + GameWorld.CameraPosition.
            //However, the highlight of provinces would then also need changing as it currently stops working with the current logic
            //scale = MapHandler.ProvinceScale;
            position = new Vector2(MapHandler.ProvinceSize * arrayPosition.X, MapHandler.ProvinceSize * arrayPosition.Y) + MapHandler.Offset;
            ProvinceRect = new Rectangle((int)Position.X, (int)Position.Y, MapHandler.ProvinceSize, MapHandler.ProvinceSize);
            UpdateCapitalText();
        }

        public void UpdateCapitalText()
        {
            foreach (UIElement ui in UIHandler.PlayerTextList)
            {
                if (ui.Type == "playerText" && ui.Name == this.Owner && this.IsCapital)
                {
                    ui.Position = this.Position;
                    if (GameWorld.ZoomScale > 1.5)
                    {
                        ui.Position += new Vector2(-GameWorld.Arial.MeasureString(ui.StaticText).X / 2, -10);

                    }
                    else
                    {
                        ui.Position += new Vector2(-GameWorld.Arial.MeasureString(ui.Text).X / 2, -10 * GameWorld.ZoomScale);
                    }
                    ui.StaticText = String.Format("{0} {1} {2}", prefix, name, suffix);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            CheckHighlight(); //Runs highlight and click code
            //TODO: production
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, Position, null, Color, rotation, origin, scale * GameWorld.ZoomScale, SpriteEffects.None, layerDepth);

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
                if (provinceRect.Contains(InputHandler.MouseStateProp.Position))
                {
                    MapHandler.HightlightProp.Position = position;
                    MapHandler.HightlightProp.HoverProvince = this;
                    if (InputHandler.MouseStateProp.LeftButton == ButtonState.Pressed)
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
            prefix = GenerateName("prefix");
            name = GenerateName("name");
            suffix = GenerateName("suffix");

            this.SetBonus(3, "Village");
            this.SetBonus(1, "Town");
        }

        /// <summary>
        /// Returns string of place names of type: prefix, name and suffix
        /// </summary>
        /// <param name="type">prefix, name, suffix</param>
        /// <returns></returns>
        private string GenerateName(string type)
        {
            //Only give the name an infix if it uses a generic name.
            //This is because the names in the realNameList already might have an infix
            int randomName = random.Next(0, MapHandler.GenericNameList.Count);
            int randomRealName = random.Next(0, MapHandler.RealNameList.Count);
            int randomPrefix = random.Next(0, MapHandler.PrefixList.Count);
            int randomInfix = random.Next(0, MapHandler.GenericInfixList.Count);
            int randomSuffix = random.Next(0, MapHandler.SuffixList.Count);
            if (type == "name")
            {
                int realNameChance = random.Next(0, 100);
                if (realNameChance < 4)
                {
                    return MapHandler.RealNameList[randomRealName];
                } else
                {
                    int infixChance = random.Next(0, 100);
                    if (infixChance < 80)
                    {
                        return MapHandler.GenericNameList[randomName] + MapHandler.GenericInfixList[randomInfix];
                    }
                    else
                    {
                        return MapHandler.GenericNameList[randomName];
                    }
                }
            }
            else if (type == "prefix")
            {
                //20% chance of prefix
                int prefixChance = random.Next(0, 100);
                if (prefixChance < 30)
                {
                    return MapHandler.PrefixList[randomPrefix];
                } else

                return "";
            }
            else if (type == "suffix")
            {
                //50% chance of suffix
                int prefixChance = random.Next(0, 100);
                if (prefixChance < 10)
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
        /// Changes the province to a bonus province based on chance and type
        /// <para>Chances the suffix</para>
        /// </summary>
        /// <param name="chance">Chance of province getting the bonus 0-100%</param>
        /// <param name="type">Type of bonus, such as "city"</param>
        public void SetBonus(int chance, string type)
        {
            int randomNumber = random.Next(0, 101);
            string t = type.ToLower();
            if (t == "capital")
            {
                this.isCapital = true;
                this.sprite = MapHandler.Sprites[3];
                this.suffix = "City";
                this.bonus = true;
                this.bonusDistance = 3;
                UpdateCapitalText();
            }
            if (t == "town" && randomNumber < chance)
            {
                this.sprite = MapHandler.Sprites[2];
                this.suffix = "Town";
                this.bonus = true;
                this.bonusDistance = 2;
            }
            if (t == "village" && randomNumber < chance)
            {
                this.sprite = MapHandler.Sprites[1];
                this.suffix = "Village";
                this.bonus = true;
                this.bonusDistance = 1;
            }
            if (this.Owner != null)
            {
                canSpread = true;
            }
            MapHandler.GeneratingMap = true;
        }
    }
}

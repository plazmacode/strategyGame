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

        public Province(int x, int y, Texture2D sprite)
        {
            ArrayPosition = new Vector2(x, y);
            random = new Random();
            Name = GenerateName();
            this.sprite = sprite;
            scale = 1.0f;
            layerDepth = 0.2f;
            Color = Color.White;
            controlledSince = 0; //0 means it is unowned
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
            ProvinceRect = new Rectangle((int)Position.X, (int)Position.Y, MapHandler.ProvinceSize, MapHandler.ProvinceSize);
        }

        public override void Update(GameTime gameTime)
        {
            CheckHighlight(); //Runs highlight and click code
            //TODO: production
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, Position, null, Color, rotation, origin, scale, SpriteEffects.None, layerDepth);
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

        private void OnClick()
        {
            MapHandler.HightlightProp.SelectedProvince = this;
        }

        private string GenerateName()
        {
            int randomNumber = random.Next(0, MapHandler.NameList.Count);

            if (randomNumber == 0)
            {
                Bonus = true;
                BonusDistance = 2;
            }

            return MapHandler.NameList[randomNumber];
        }

        public void SetCapital()
        {
            this.bonus = true;
            this.bonusDistance = 3;
        }

    }
}

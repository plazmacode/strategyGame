using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace strategyGame.Classes
{
    public class Hightlight : GameObject
    {
        private Province hoverProvince;
        private Province selectedProvince;

        public Hightlight()
        {
            layerDepth = 0.8f;
            scale = 1.0f;
        }

        internal Province SelectedProvince { get => selectedProvince; set => selectedProvince = value; }
        internal Province HoverProvince { get => hoverProvince; set => hoverProvince = value; }

        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("highlight");
        }

        public override void OnCollision(GameObject other)
        {

        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, Position, null, Color.White, rotation, origin, MapHandler.ProvinceScale, SpriteEffects.None, layerDepth);
        }

    }
}

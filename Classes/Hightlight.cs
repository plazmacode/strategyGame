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
        private float highlightScale;

        public Hightlight()
        {
            layerDepth = 0.5f;
            scale = 1f;
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

        /// <summary>
        /// Only used when the size of provinces or zoomScale is resized, the position i always updated
        /// </summary>
        public void OnResize()
        {
            if (MapHandler.Sprites[0].Name.Contains("Small"))
            {
                highlightScale = 0.5f;
            }
            scale = highlightScale * MapHandler.ProvinceScale * GameWorld.ZoomScale;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, Position, null, Color.White, rotation, origin, scale, SpriteEffects.None, layerDepth);
        }

    }
}

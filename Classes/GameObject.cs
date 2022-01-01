using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace strategyGame
{
    public abstract class GameObject
    {
        protected Vector2 position;
        protected Vector2 origin;
        protected Texture2D sprite;
        protected Color color;
        protected float rotation;
        protected float scale;
        protected float layerDepth;

        public Vector2 Position { get => position; set => position = value; }

        public abstract void LoadContent(ContentManager content);

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, Position, null, Color.White, rotation, origin, scale, SpriteEffects.None, layerDepth);
        }

        public abstract void OnCollision(GameObject other);
    }
}

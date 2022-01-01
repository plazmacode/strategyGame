using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace strategyGame.Classes
{
   public  class UIElement
    {
        private Vector2 position;
        private Rectangle collisionBox;
        private string name;
        private string staticText; //Text that stays the same ~always
        private string text;
        private string type;

        public string Name { get => name; set => name = value; }
        public string Text { get => text; set => text = value; }
        public string Type { get => type; set => type = value; }
        public Vector2 Position { get => position; set => position = value; }
        public Rectangle CollisionBoxProp { get => collisionBox; set => collisionBox = value; }
        public string StaticText { get => staticText; set => staticText = value; }

        public UIElement(string staticText, string name, string type, int x, int y)
        {
            this.name = name;
            this.type = type;
            this.staticText = staticText;
            SetPosition(text, x, y);
        }

        public UIElement(string staticText, string text, string name, string type, int x, int y)
        {
            this.name = name;
            this.type = type;
            this.text = text;
            this.staticText = staticText;
            SetPosition(text, x, y);
        }

        //TODO: add UIarea for better positioning inside Rectangle. Fixes most onResize issues.
        //GameWorld.Arial.MeasureString(text)
        private void SetPosition(string text, int x, int y)
        {
            Position = new Vector2(x, y);
            collisionBox = new Rectangle((int)Position.X, (int)Position.Y, 200, 24);

        }
        public void UpdatePosition()
        {
            //Position += GameWorld.ScreenSize / 2 - GameWorld.OldScreenSize / 2;
        }
    }
}

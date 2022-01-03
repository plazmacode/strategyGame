using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace strategyGame.Classes
{
    /// <summary>
    /// This class loads and updates UI
    /// </summary>
    public static class UIHandler
    {
        private static Province selected;
        private static MouseState oldState;
        private static List<UIElement> UIList = new List<UIElement>();
        private static List<UIElement> playerTextList = new List<UIElement>();
        public static List<UIElement> UIListProp { get => UIList; set => UIList = value; }
        public static List<UIElement> PlayerTextList { get => playerTextList; set => playerTextList = value; }

        public static void LoadUI()
        {
            UIList.Add(new UIElement("Province: ", "provinceName", "text", 0, 80));
            UIList.Add(new UIElement("Coords: ", "provinceCoords", "text", 0, 96));
            UIList.Add(new UIElement("Owner: ", "ownerName", "text", 0, 112));
            UIList.Add(new UIElement("Controlled since: ", "controlledText", "text", 0, 128));
            UIList.Add(new UIElement("Bonus: ", "bonusText", "text", 0, 144));
            UIList.Add(new UIElement("Bonus Distance: ", "bonusDistanceText", "text", 0, 160));

            UIList.Add(new UIElement("Add Bonus", "bonusButton", "button", 0, 180));
        }

        public static void UpdateUI()
        {
            //TODO make this code better looking/running
            selected = MapHandler.HightlightProp.SelectedProvince;

            foreach (UIElement ui in UIList)
            {
                if (ui.Name == "provinceName" && selected != null)
                {
                    if (selected.Prefix != "")
                    {
                        ui.Text = String.Format("{0} {1} {2}", selected.Prefix, selected.Name, selected.Suffix);
                    } else
                    {
                        ui.Text = String.Format("{1} {2}", selected.Prefix, selected.Name, selected.Suffix);
                    }
                }

                if (ui.Name == "provinceCoords" && selected != null)
                {
                    ui.Text = "Coords: " + selected.ArrayPosition.ToString();
                }

                if (ui.Name == "ownerName" && selected != null)
                {
                    ui.Text = selected.Owner;
                    if (ui.Text == null)
                    {
                        ui.Text = "wasteland";
                    }
                }

                if (ui.Name == "controlledText" && selected != null)
                {
                    ui.Text = selected.ControlledSince.ToString();
                    if (ui.Text == null)
                    {
                        ui.Text = "uncontrolled";
                    }
                }

                if (ui.Name == "bonusText" && selected != null)
                {
                    ui.Text = selected.Bonus.ToString();
                }

                if (ui.Name == "bonusDistanceText" && selected != null)
                {
                    ui.Text = selected.BonusDistance.ToString();
                }

                if (ui.Name == "bonusButton" && selected != null)
                {
                    if (InputHandler.MouseStateProp.LeftButton == ButtonState.Pressed && ui.CollisionBoxProp.Contains(InputHandler.MouseStateProp.Position))
                    {
                        selected.SetBonus(100, "town");
                    }                    
                }

                if (selected == null)
                {
                    ui.Text = "";
                }
            }
            oldState = InputHandler.MouseStateProp;
        }

        public static void DrawTexts(SpriteBatch spriteBatch)
        {
            GameWorld.DrawRect(new Rectangle(0, 0, 320, 300), Color.Black * 0.4f, 0.5f);
            //spriteBatch.DrawString(GameWorld.Arial, );
            foreach (UIElement ui in UIList)
            {
                spriteBatch.DrawString(GameWorld.Arial, ui.StaticText + ui.Text, ui.Position, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.7f);
            }
            foreach (UIElement ui in playerTextList)
            {
                if (GameWorld.ZoomScale > 1.5)
                {
                    //Show capital name
                    GameWorld.DrawRect(new Rectangle((int)ui.Position.X, (int)ui.Position.Y, (int)GameWorld.Arial.MeasureString(ui.StaticText).X + 12, 24), Color.Gray * 0.5f, 0.6f);
                    spriteBatch.DrawString(GameWorld.Arial, ui.StaticText, ui.Position, Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.7f);
                }
                else
                {
                    //Show capital owner name
                    GameWorld.DrawRect(new Rectangle((int)ui.Position.X, (int)ui.Position.Y, (int)GameWorld.Arial.MeasureString(ui.Text).X + 12, 24), Color.Gray * 0.5f, 0.6f);
                    spriteBatch.DrawString(GameWorld.Arial, ui.Text, ui.Position, Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.7f);
                }
            }
        }
    }
}

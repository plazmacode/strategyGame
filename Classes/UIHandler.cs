using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace strategyGame.Classes
{
    public static class UIHandler
    {
        private static MouseState oldState;

        public static void LoadUI()
        {
            GameWorld.UIListProp.Add(new UIElement("Province: ", "provinceName", "text", 0, 80));
            GameWorld.UIListProp.Add(new UIElement("Coords: ", "provinceCoords", "text", 0, 96));
            GameWorld.UIListProp.Add(new UIElement("Owner: ", "ownerName", "text", 0, 112));
            GameWorld.UIListProp.Add(new UIElement("Controlled since: ", "controlledText", "text", 0, 128));
            GameWorld.UIListProp.Add(new UIElement("Bonus: ", "bonusText", "text", 0, 144));
            GameWorld.UIListProp.Add(new UIElement("Bonus Distance: ", "bonusDistanceText", "text", 0, 1160));

        }

        public static void UpdateText()
        {
            //TODO make this code better looking
            foreach (UIElement ui in GameWorld.UIListProp)
            {
                Province selected = MapHandler.HightlightProp.SelectedProvince;
                if (ui.Name == "provinceName" && selected != null)
                {
                    ui.Text = selected.Name;
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
                    ui.Text = "Bonus: " + selected.Bonus.ToString();
                }

                if (ui.Name == "bonusDistanceText" && selected != null)
                {
                    ui.Text = "Bonus Distance: " + selected.BonusDistance.ToString();
                }

                if (selected == null)
                {
                    ui.Text = "";
                }

            }
        }
    }
}

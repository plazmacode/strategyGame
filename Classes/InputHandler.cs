using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace strategyGame.Classes
{
    /// <summary>
    /// This class handles all the input. Keyboard, mouse...
    /// </summary>
    public static class InputHandler
    {
        private static MouseState mouseState;
        private static MouseState oldState;
        private static KeyboardState keyState;
        private static KeyboardState oldKeyState;

        private static Vector2 draggingPosition;
        private static float cameraSpeed;


        public static MouseState MouseStateProp { get => mouseState; set => mouseState = value; }
        public static MouseState OldState { get => oldState; set => oldState = value; }
        public static KeyboardState KeyStateProp { get => keyState; set => keyState = value; }
        public static KeyboardState OldKeyState { get => oldKeyState; set => oldKeyState = value; }

        public static Vector2 DraggingPosition { get => draggingPosition; set => draggingPosition = value; }


        static InputHandler()
        {
            cameraSpeed = 10;
        }


        public static void Update()
        {
            KeyStateProp = Keyboard.GetState();
            mouseState = Mouse.GetState();

            if (keyState.IsKeyDown(Keys.Space))
            {
                if (MapHandler.HightlightProp.SelectedProvince != null)
                {
                    MapHandler.HightlightProp.SelectedProvince.SetBonus(100, "town");
                }
            }

            if (oldKeyState.IsKeyUp(Keys.R) && keyState.IsKeyDown(Keys.R))
            {
                MapHandler.ClearMap();
                MapHandler.Build();
            }
            CameraInput();
        }

        public static void CameraInput()
        {
            //Scrolling
            if (mouseState.ScrollWheelValue > oldState.ScrollWheelValue)
            {
                GameWorld.ZoomScale *= 1.1f;
                MapHandler.OnResize();
                MapHandler.HightlightProp.OnResize();
                foreach (Province province in MapHandler.Map)
                {
                    province.OnResize();
                }
            }
            else if (mouseState.ScrollWheelValue < oldState.ScrollWheelValue)
            {
                GameWorld.ZoomScale /= 1.1f;
                MapHandler.OnResize();
                MapHandler.HightlightProp.OnResize();
                foreach (Province province in MapHandler.Map)
                {
                    province.OnResize();
                }
            }

            //Mouse dragging
            if (mouseState.MiddleButton == ButtonState.Pressed)
            {
                draggingPosition = new Vector2(oldState.X - mouseState.Position.X,
                    oldState.Y - mouseState.Position.Y);
                GameWorld.CameraPosition -= draggingPosition / GameWorld.ZoomScale;
                MapHandler.OnResize();
                UpdateOffsets();
            }

            ///Arrow Keys & WASD
            if (keyState.IsKeyDown(Keys.Left))
            {
                GameWorld.CameraPosition = new Vector2(GameWorld.CameraPosition.X + cameraSpeed, GameWorld.CameraPosition.Y);
                MapHandler.OnResize();
                UpdateOffsets();
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                GameWorld.CameraPosition = new Vector2(GameWorld.CameraPosition.X - cameraSpeed, GameWorld.CameraPosition.Y);
                MapHandler.OnResize();
                UpdateOffsets();
            }
            if (keyState.IsKeyDown(Keys.Up))
            {
                GameWorld.CameraPosition = new Vector2(GameWorld.CameraPosition.X, GameWorld.CameraPosition.Y + cameraSpeed);
                UpdateOffsets();
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                GameWorld.CameraPosition = new Vector2(GameWorld.CameraPosition.X, GameWorld.CameraPosition.Y - cameraSpeed);
                UpdateOffsets();                
            }

            if (keyState.IsKeyDown(Keys.A))
            {
                GameWorld.CameraPosition = new Vector2(GameWorld.CameraPosition.X + cameraSpeed, GameWorld.CameraPosition.Y);
                MapHandler.OnResize();
                UpdateOffsets();
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                GameWorld.CameraPosition = new Vector2(GameWorld.CameraPosition.X - cameraSpeed, GameWorld.CameraPosition.Y);
                MapHandler.OnResize();
                UpdateOffsets();
            }
            if (keyState.IsKeyDown(Keys.W))
            {
                GameWorld.CameraPosition = new Vector2(GameWorld.CameraPosition.X, GameWorld.CameraPosition.Y + cameraSpeed);
                UpdateOffsets();
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                GameWorld.CameraPosition = new Vector2(GameWorld.CameraPosition.X, GameWorld.CameraPosition.Y - cameraSpeed);
                UpdateOffsets();
            }

            void UpdateOffsets()
            {
                MapHandler.OnResize();
                foreach (Province province in MapHandler.Map)
                {
                    province.OnResize();
                }
            }
            oldState = mouseState;
            oldKeyState = keyState;
        }
    }
}

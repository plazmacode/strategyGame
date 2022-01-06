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
        private static Vector2 cameraVelocity;


        public static MouseState MouseStateProp { get => mouseState; set => mouseState = value; }
        public static MouseState OldState { get => oldState; set => oldState = value; }
        public static KeyboardState KeyStateProp { get => keyState; set => keyState = value; }
        public static KeyboardState OldKeyState { get => oldKeyState; set => oldKeyState = value; }

        public static Vector2 DraggingPosition { get => draggingPosition; set => draggingPosition = value; }
        public static float CameraSpeed { get => cameraSpeed; set => cameraSpeed = value; }

        static InputHandler()
        {
            CameraSpeed = 500;
        }


        public static void Update(GameTime gameTime)
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
            CameraMovement(gameTime);
        }

        private static void CameraMovement(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Update camera position
            GameWorld.CameraPosition += cameraVelocity * CameraSpeed * deltaTime;

            //Update OnResize positions
            if (cameraVelocity != Vector2.Zero)
            {
                MapHandler.OnResize();
                foreach (Province province in MapHandler.Map)
                {
                    province.OnResize();
                }
            }
            cameraVelocity = Vector2.Zero;
        }

        /// <summary>
        /// Camera input and camera zoom
        /// </summary>
        public static void CameraInput()
        {
            void UpdateOffset()
            {
                MapHandler.OnResize();
                foreach (Province province in MapHandler.Map)
                {
                    province.OnResize();
                }
            }

            //Scrolling for camera zoom
            if (mouseState.ScrollWheelValue > oldState.ScrollWheelValue)
            {
                GameWorld.ZoomScale *= 1.1f;
                MapHandler.HightlightProp.OnResize();
                UpdateOffset();
            }
            else if (mouseState.ScrollWheelValue < oldState.ScrollWheelValue)
            {
                GameWorld.ZoomScale /= 1.1f;
                MapHandler.HightlightProp.OnResize();
                UpdateOffset();
            }

            //Mouse dragging
            if (mouseState.MiddleButton == ButtonState.Pressed)
            {
                draggingPosition = new Vector2(oldState.X - mouseState.Position.X,
                    oldState.Y - mouseState.Position.Y);
                GameWorld.CameraPosition -= draggingPosition / GameWorld.ZoomScale;
                UpdateOffset();

            }

            ///Arrow Keys & WASD
            if (keyState.IsKeyDown(Keys.Left))
            {
                cameraVelocity += new Vector2(1, 0);
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                cameraVelocity += new Vector2(-1, 0);
            }
            if (keyState.IsKeyDown(Keys.Up))
            {
                cameraVelocity += new Vector2(0, 1);
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                cameraVelocity += new Vector2(0, -1);
            }

            if (keyState.IsKeyDown(Keys.A))
            {
                cameraVelocity += new Vector2(1, 0);
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                cameraVelocity += new Vector2(-1, 0);
            }
            if (keyState.IsKeyDown(Keys.W))
            {
                cameraVelocity += new Vector2(0, 1);
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                cameraVelocity += new Vector2(0, -1);
            }
            oldState = mouseState;
            oldKeyState = keyState;
        }
    }
}

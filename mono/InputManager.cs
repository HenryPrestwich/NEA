using Microsoft.Xna.Framework.Input;

namespace mono
{
    public static class IM
    {
        public static KeyboardState previousKBS;
        public static KeyboardState currentKBS = Keyboard.GetState();

        public static MouseState previousMS;
        public static MouseState currentMS = Mouse.GetState();

        public static void UpdateInput()
        {
            previousKBS = currentKBS;
            currentKBS = Keyboard.GetState();

            previousMS = currentMS;
            currentMS = Mouse.GetState();
        }

        public static bool KeyDown(Keys key)
        {
            return currentKBS.IsKeyDown(key);
        }
        public static bool KeyJustPressed(Keys key)
        {
            bool JustPressed = false;
            if (currentKBS.IsKeyDown(key) && previousKBS.IsKeyUp(key))
            {
                JustPressed = true;
            }
            return JustPressed;
        }

        public static bool KeUp(Keys key)
        {
            return currentKBS.IsKeyUp(key);
        }


        public static bool IsMouseClick(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return currentMS.LeftButton == ButtonState.Pressed;
                case MouseButton.Middle:
                    return currentMS.MiddleButton == ButtonState.Pressed;
                case MouseButton.Right:
                    return currentMS.RightButton == ButtonState.Pressed;
                default: 
                    return false;

            }
        }
    }

    
    public enum MouseButton
    {
        Left,
        Middle,
        Right
    }
}

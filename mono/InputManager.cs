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
        }

        public static bool IsKeyDown(Keys key)
        {
            return currentKBS.IsKeyDown(key);
        }
        public static bool IsKeUp(Keys key)
        {
            return currentKBS.IsKeyUp(key);
        }
    }
}

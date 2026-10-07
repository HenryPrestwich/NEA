using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core.Tokens;

namespace mono
{
    public static class InputManager
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

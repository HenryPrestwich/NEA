using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}

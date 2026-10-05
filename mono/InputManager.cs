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
        public static KeyboardState currentKBS;

        public static MouseState previousMS;
        public static MouseState currentMS;
    }
}

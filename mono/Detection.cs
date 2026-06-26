using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mono
{
    internal class Detection
    {



        public static Rectangle RectCalc(Vector2 position, Vector2 size)
        {
            Rectangle rect =new Rectangle(Convert.ToInt32(position.X - size.X / 2), Convert.ToInt32(position.Y - size.Y / 2), Convert.ToInt32(size.X), Convert.ToInt32(size.Y));

            return rect;
        }
    }
}

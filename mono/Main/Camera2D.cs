using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace mono.Main
{
    internal class Camera2D
    {
        
        private Viewport viewport;

        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; }



        public Camera2D(Viewport viewport)
        {
            this.viewport = viewport;
            Scale = 1.0f;
        }

        public Matrix GetCamMatrix()
        {
            return
                Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Scale, Scale, 1f) *
                Matrix.CreateTranslation(viewport.Width / 2f, viewport.Height / 2f, 0);   
        }

        public void Track(Vector2 Plocate)
        {
            Position = Vector2.Lerp(Position, Plocate, 0.35f);
        }
    }
}

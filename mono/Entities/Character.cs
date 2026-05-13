using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mono.Main;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.IO;

namespace mono.Entities
{
    public class Character
    {
        public int HP { get; set; }
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Centre { get; set; }

        public Character(Texture2D texture)
        {
            HP = 0;
            Position = new Vector2(56, 56);
            Texture = texture;
            Centre = new Vector2(texture.Width / 2, texture.Height / 2);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Centre, 1.5f, SpriteEffects.None, Layers.Entity);
        }
        public virtual void Move()
        {

        }
        public virtual void Move(KeyboardState KB, GamePadState GP)
        {

        }
    }
}

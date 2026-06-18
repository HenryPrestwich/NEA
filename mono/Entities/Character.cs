using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mono.Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace mono.Entities
{
    public class Character
    {
        public int HP { get; set; }
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Centre { get; set; }
        public int Speed    { get; set; }
        public Rectangle Rectangle { get; set; }


        public Character(Texture2D texture, Vector2 Position)
        {
            HP = 0;
            this.Position = Position;
            Texture = texture;
            Centre = new Vector2(texture.Width / 2, texture.Height / 2);
            Speed = 0;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Centre, 1.5f, SpriteEffects.None, Layers.Entity);
        }
        public virtual void Move(Player player)
        {

        }
        public virtual void Move(KeyboardState KB, GamePadState GP, List<Character> charlist)
        {

        }
        public void updateRect()
        {
            this.Rectangle = new Rectangle(Convert.ToInt32(this.Position.X), Convert.ToInt32(this.Position.Y), this.Texture.Width, this.Texture.Height);
        }

        public void DrawRect(SpriteBatch spriteBatch, Texture2D pixel)
        {
           
            spriteBatch.Draw(pixel, new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, 1), Color.Red);
            
            spriteBatch.Draw(pixel, new Rectangle(Rectangle.X, Rectangle.Y + Rectangle.Height - 1, Rectangle.Width, 1), Color.Red);
            
            spriteBatch.Draw(pixel, new Rectangle(Rectangle.X, Rectangle.Y, 1, Rectangle.Height), Color.Red);
            
            spriteBatch.Draw(pixel, new Rectangle(Rectangle.X + Rectangle.Width - 1, Rectangle.Y, 1, Rectangle.Height), Color.Red);
        }
    }
}

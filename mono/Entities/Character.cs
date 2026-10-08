using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;


namespace mono.Entities
{
    public class Character
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 Centre { get; set; }
        public Rectangle Rectangle { get; set; }


        public int Speed { get; set; }
        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int Range { get; set; }
        public int Damage { get; set; }
        public int AttackSpeed { get; set; }
        public int AttackCooldown {  get; set; }
        public int ProjectileSpeed { get; set; }


        public Character(Vector2 position)
        {
            this.HP = 0;
            this.Position = position;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Centre, 1f, SpriteEffects.None, Layers.Entity);
        }
        public void updateRect()
        {
            Rectangle rect = RectCalc(this.Position, this.Size);
            this.Rectangle = rect;
        }

        public static Rectangle RectCalc(Vector2 position, Vector2 size)
        {
            Rectangle rect = new Rectangle(Convert.ToInt32(position.X - size.X / 2), Convert.ToInt32(position.Y - size.Y / 2), Convert.ToInt32(size.X), Convert.ToInt32(size.Y));
            return rect;
        }


        public virtual void DamageEntity(int damage)
        {
            this.HP -= damage;
        }


        public void DrawRect(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.GetTexture("pixel"), this.Position, Color.Red);

            spriteBatch.Draw(TextureManager.GetTexture("pixel"), new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, 1), Color.Red);

            spriteBatch.Draw(TextureManager.GetTexture("pixel"), new Rectangle(Rectangle.X, Rectangle.Y + Rectangle.Height - 1, Rectangle.Width, 1), Color.Red);

            spriteBatch.Draw(TextureManager.GetTexture("pixel"), new Rectangle(Rectangle.X, Rectangle.Y, 1, Rectangle.Height), Color.Red);

            spriteBatch.Draw(TextureManager.GetTexture("pixel"), new Rectangle(Rectangle.X + Rectangle.Width - 1, Rectangle.Y, 1, Rectangle.Height), Color.Red);
        }
    }
}

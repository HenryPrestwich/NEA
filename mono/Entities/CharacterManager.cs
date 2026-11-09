using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mono.Entities
{
    public static class CM
    {
        public static List<Enemy> EnemyList { get; private set; } = new List<Enemy>();
        public static Player Player { get; private set; }


        public static void CreatePlayer(Vector2 position)
        {
            if (Player == null)
            {
                Player = new Player(position);
            }
        }

        public static void CreateEnemy(Vector2 position)
        {
            EnemyList.Add(new Enemy(position));
        }

        public static void DrawCharacters(SpriteBatch spritebatch)
        {
            Player.Draw(spritebatch);
            foreach (Enemy enemy in EnemyList)
            {
                enemy.Draw(spritebatch);
            }
        }
    }


    public abstract class Character
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
        public int AttackCooldown { get; set; }
        public int ProjectileSpeed { get; set; }


        public Character(Vector2 position)
        {
            this.Position = position;
        }
        public  void Draw(SpriteBatch spriteBatch)
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

        public abstract void DamageEntity(int damage);

        public void DrawRect(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TM.GetTexture("pixel"), this.Position, Color.Red);

            spriteBatch.Draw(TM.GetTexture("pixel"), new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, 1), Color.Red);

            spriteBatch.Draw(TM.GetTexture("pixel"), new Rectangle(Rectangle.X, Rectangle.Y + Rectangle.Height - 1, Rectangle.Width, 1), Color.Red);

            spriteBatch.Draw(TM.GetTexture("pixel"), new Rectangle(Rectangle.X, Rectangle.Y, 1, Rectangle.Height), Color.Red);

            spriteBatch.Draw(TM.GetTexture("pixel"), new Rectangle(Rectangle.X + Rectangle.Width - 1, Rectangle.Y, 1, Rectangle.Height), Color.Red);
        }
    }
}

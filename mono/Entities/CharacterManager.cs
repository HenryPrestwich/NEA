using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace mono.Entities
{
    public static class CM
    {
        public static List<Enemy> EnemyList { get; private set; } = new List<Enemy>();
        public static Player Player { get; private set; }


        public static void CreatePlayer(Vector2 position, CharacterType playerType)
        {
            if (Player == null)
            {
                Player = new Player(position, DM.GetData(playerType));
            }
        }

        public static void CreateEnemy(Vector2 position, CharacterType enemyType)
        {
            EnemyList.Add(new Enemy(position, DM.GetData(enemyType)));
        }

        public static void DrawCharacters(SpriteBatch spritebatch)
        {
            Player.Draw(spritebatch);
            foreach (Enemy enemy in EnemyList)
            {
                enemy.Draw(spritebatch);
            }
        }

        public static void UpdateCharacters(int gameClock, Map map)
        {
            if (gameClock % 15 == 0)
            {
                foreach (Enemy enemy in EnemyList)
                {
                    enemy.SetPath(map);
                }
            }

            for (int i = 0; i < EnemyList.Count; i++)
            {
                Enemy e = EnemyList[i];
                e.Move();
                e.updateRect();
                e.SetState();
                if (e.state == EnemyState.DEAD)
                {
                    EnemyList.Remove(e);
                }
            }


            Player.Move(map);
            Player.Attack();
        }
    }


    public abstract class Character
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 Centre { get; set; }
        public Rectangle Rectangle { get; set; }

        private int speed;
        public int Speed 
        { 
            get 
            {
                if (this.Effects.Contains(CharacterEffect.Slow))
                {
                    return ((int)(this.speed * 0.8));
                }
                return this.speed;
            }
            set => speed = value;
        } 
        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int Range { get; set; }
        public int Damage { get; set; }
        public int AttackSpeed { get; set; }
        public int AttackCooldown { get; set; }
        public int ProjectileSpeed { get; set; }

        public Dictionary<CharacterEffect, bool> Effects { get; set; }


        public Character(Vector2 position, CharacterData data)
        {
            this.Position = position;
            this.Speed = data.Speed;
            this.MaxHP = data.MaxHP;
            this.HP = MaxHP;
            this.Range = data.Range;
            this.Damage = data.Damage;
            this.AttackSpeed = data.AttackSpeed;
            this.ProjectileSpeed = data.ProjectileSpeed;

            AttackCooldown = data.AttackSpeed;

            this.Texture = TM.GetTexture(data.Texture);
            this.Centre = new Vector2(Texture.Width / 2, Texture.Height / 2);
            this.Size = new Vector2(Texture.Width, Texture.Height);

            for (int i = 0; i < (int)CharacterEffect.count; i++)
            {
                Effects.Add((CharacterEffect)i, false);
            }
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

        public abstract void Attack();
        public abstract void DamageEntity(int damage, Vector2 position);

        public void DrawRect(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TM.GetTexture("pixel"), this.Position, Color.Red);

            spriteBatch.Draw(TM.GetTexture("pixel"), new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, 1), Color.Red);

            spriteBatch.Draw(TM.GetTexture("pixel"), new Rectangle(Rectangle.X, Rectangle.Y + Rectangle.Height - 1, Rectangle.Width, 1), Color.Red);

            spriteBatch.Draw(TM.GetTexture("pixel"), new Rectangle(Rectangle.X, Rectangle.Y, 1, Rectangle.Height), Color.Red);

            spriteBatch.Draw(TM.GetTexture("pixel"), new Rectangle(Rectangle.X + Rectangle.Width - 1, Rectangle.Y, 1, Rectangle.Height), Color.Red);
        }
    }


    public enum CharacterEffect
    {
        Slow,
        Fire,
        Trap,

        
        
        count
    }

    public enum CharacterType
    {
        Evil = 0,
        Bat,
        Cheese,
        Boss,



        Marvin
    }
}

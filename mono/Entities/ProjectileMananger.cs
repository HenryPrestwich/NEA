using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;


namespace mono.Entities
{
    public static class ProjectileManager
    {
        private static List<Projectile> Projectiles = new List<Projectile>();


        public static void AddProjectile(Vector2 position, int speedX, int speedY, int range, Owner owner)
        {
            Projectiles.Add(new Projectile(position, speedX, speedY, range, owner));
        }

        public static void RemoveProjectile(Projectile p)
        {
            Projectiles.Remove(p);
        }

        public static void DrawProjectiles(SpriteBatch spriteBatch)
        {
            foreach (Projectile p in Projectiles)
            {
                p.DrawProjectile(spriteBatch);
            }
        }
        public static void UpdateProjectiles(Player player, List<Enemy> eList, Map map)
        {
            for (int i = 0; i < Projectiles.Count; i++)
            {
                Projectile p = Projectiles[i];
                p.MoveProjectile();
                if (p.RemainingTime == 0 || p.Pierce == 0)
                {
                    Projectiles.Remove(p);
                }
                else
                {
                    p.CheckCollisions(player, eList, map);
                }
            }
        }
        

        public static void DrawHitboxes(SpriteBatch spriteBatch)
        {
            foreach (Projectile p in Projectiles)
            {
                p.DrawHitbox(spriteBatch);
            }
        }
    }
    public class Projectile
    {
        public Owner Owner { get; private set; }
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; set; }
        public Rectangle Rectangle { get; private set; }
        public Vector2 Size { get; set; }
        public Vector2 Centre { get; set; }
        public Vector2 Velocity { get; set; }
        public List<Enemy> HasHit {  get; private set; }
        public int Damage { get; set; }
        public int RemainingTime { get; set; }
        public int Pierce { get; set; }
        float Rotation { get; set; }

        public Projectile(Vector2 position, int speedX, int speedY, int range, Owner owner)
        {
            this.Texture = TextureManager.GetTexture("projectile");

            Pierce = 1;
            Damage = 1;
            this.Position = position;
            this.Centre = new Vector2(8, 8);
            this.Size = new Vector2(Texture.Width, Texture.Height);

            this.Velocity = new Vector2(speedX, speedY);
            this.RemainingTime = range;

            this.Rotation = 0f;

            HasHit = new List<Enemy>();

            this.Owner = owner;
        }
        public void MoveProjectile()
        {
            Position = Position + Velocity;
            Rotation += 0.5f;
            RemainingTime -= 1;

            UpdateRect();
        }
        public void UpdateRect()
        {
            Rectangle rect = RectCalc(this.Position, this.Size);
            this.Rectangle = rect;
        }

        public static Rectangle RectCalc(Vector2 position, Vector2 size)
        {
            Rectangle rect = new Rectangle(Convert.ToInt32(position.X - size.X / 2), Convert.ToInt32(position.Y - size.Y / 2), Convert.ToInt32(size.X), Convert.ToInt32(size.Y));
            return rect;
        }

        public void CheckCollisions(Player player, List<Enemy> eList, Map map)
        {
            bool removed = false;


            List<Node> nodes = new List<Node>();
            Node n = map.Grid[(int)Position.X / 32, (int)Position.Y / 32];
            nodes.Add(n);
            foreach (Node a in n.Neigbour)
            {
                nodes.Add(a);
            }
            foreach (Node a in nodes)
            {
                if (Rectangle.Intersects(a.Rectangle) && a.Walkable == false)
                {
                    ProjectileManager.RemoveProjectile(this);
                    removed = true;
                    break;
                }
            }
            if (removed == false && Owner != Owner.enemy)
            {
                foreach (Enemy e in eList)
                {
                    if (!HasHit.Contains(e) && Rectangle.Intersects(e.Rectangle))
                    {
                        e.DamageEntity(Damage);
                        HasHit.Add(e);
                        Pierce -= 1;
                    }
                }
            }
            if (removed == false && Owner != Owner.player)
            {
                if (Rectangle.Intersects(player.Rectangle))
                {
                    player.DamageEntity(Damage);
                }
            }
        }

        public void DrawProjectile(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, Rotation, Centre, 1f, SpriteEffects.None, Layers.Entity);
        }
        public void DrawHitbox(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.GetTexture("pixel"), new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, 1), Color.Red);
            spriteBatch.Draw(TextureManager.GetTexture("pixel"), new Rectangle(Rectangle.X, Rectangle.Y + Rectangle.Height - 1, Rectangle.Width, 1), Color.Red);
            spriteBatch.Draw(TextureManager.GetTexture("pixel"), new Rectangle(Rectangle.X, Rectangle.Y, 1, Rectangle.Height), Color.Red);
            spriteBatch.Draw(TextureManager.GetTexture("pixel"), new Rectangle(Rectangle.X + Rectangle.Width - 1, Rectangle.Y, 1, Rectangle.Height), Color.Red);
        }
    }

    public enum Owner
    {
        player = 0,
        enemy
    }
}

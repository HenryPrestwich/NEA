using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;


namespace mono.Entities
{
    public static class ProjectileManager
    {
        public static List<Projectile> Projectiles = new List<Projectile>();


        public static void AddProjectile(Vector2 position, int speedX, int speedY, int range, Owner owner)
        {
            Projectiles.Add(new Projectile(position, speedX, speedY, range, owner));
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
            foreach (Projectile p in Projectiles)
            {
                p.MoveProjectile();
                if (p.RemainingTime == 0)
                {
                    Projectiles.Remove(p);
                }
            }
            CheckCollisions(player, eList, map);
        }
        public static void CheckCollisions(Player player, List<Enemy> eList, Map map)
        {
            if (Projectiles != null)
            {
                for (int i = 0; i < Projectiles.Count; i++)
                {
                    Projectile p = Projectiles[i];

                    List<Node> nodes = new List<Node>();
                    Node n = map.Grid[(int)p.Position.X / 32, (int)p.Position.Y / 32];
                    nodes.Add(n);
                    foreach (Node a in n.Neigbour)
                    {
                        nodes.Add(a);
                    }
                    foreach (Node a in nodes)
                    {
                        if (p.Rectangle.Intersects(a.Rectangle) && a.Walkable == false)
                        {
                            Projectiles.Remove(p);
                            break;
                        }
                    }
                }
            }
            foreach (Projectile p in Projectiles)
            {
                if (p.Owner != Owner.enemy)
                {
                    foreach (Enemy e in eList)
                    {
                        if (p.Rectangle.Intersects(e.Rectangle))
                        {
                            e.DamageEntity(p.Damage);
                            p.Pierce -= 1;
                        }
                    }
                }
                if (p.Owner != Owner.player)
                {
                    if (p.Rectangle.Intersects(player.Rectangle))
                    {
                        player.DamageEntity(p.Damage);
                    }
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

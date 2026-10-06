using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;


namespace mono.Entities
{
    public static class ProjectileManager
    {
        public static List<Projectile> Projectiles = new List<Projectile>();


        public static void AddProjectile(Vector2 position, int speedX, int speedY, int range)
        {
            Projectiles.Add(new Projectile(position, speedX, speedY, range));
        }


        public static void DrawProjectiles(SpriteBatch spriteBatch)
        {
            foreach (Projectile p in Projectiles)
            {
                p.DrawProjectile(spriteBatch);
            }
        }
        public static void UpdateProjectiles(int clock)
        {
            foreach (Projectile p in Projectiles)
            {
                p.MoveProjectile();
                if (p.RemainingTime == 0)
                {
                    Projectiles.Remove(p);
                }
            }
        }
    }
    public class Projectile
    {
        public int Owner { get; private set; }
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; set; }
        public Vector2 Centre { get; set; }
        public Vector2 Velocity { get; set; }
        public int RemainingTime { get; set; }
        float Rotation { get; set; }

        public Projectile(Vector2 position, int speedX, int speedY, int range)
        {

            this.Position = position;
            this.Centre = new Vector2(16, 16);

            this.Velocity = new Vector2(speedX, speedY);
            this.RemainingTime = range;

            this.Rotation = 0f;

            this.Texture = TextureManager.GetTexture("projectile");
        }

        public void MoveProjectile()
        {
            Position = Position + Velocity;
            Rotation += 1;
            RemainingTime -= 1;

            UpdateRect();
        }
        public void UpdateRect()
        {

        }

        public void DrawProjectile(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, Rotation, Centre, 1f, SpriteEffects.None, Layers.Entity);
        }
    }

    public enum Owners
    {
        player = 0,
        enemy
    }
}

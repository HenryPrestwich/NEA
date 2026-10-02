using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace mono.Entities
{
    public static class ProjectileManager
    {
        public static List<Projectile> Projectiles = new List<Projectile>();


        public static void DrawProjectiles(SpriteBatch spriteBatch)
        {
            foreach (Projectile p in Projectiles)
            {
                p.DrawProjectile(spriteBatch);
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
        float rotation { get; set; }

        public Projectile(Vector2 position, int speedX, int speedY, int range)
        {
            Position = position;
            Centre = Vector2.Zero;

            Velocity = new Vector2(speedX, speedY);
            RemainingTime = range;

            rotation = 0f;

            Texture = TextureManager.GetTexture("projectile");
        }

        public void MoveProjectile()
        {
            Position = Position + Velocity;
            rotation += 1;
        }
        public void UpdateRect()
        {

        }

        public void DrawProjectile(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Centre, 1f, SpriteEffects.None, Layers.Entity);
        }
    }

    public enum Owners
    {
        player = 0,
        enemy
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace mono.Entities
{
    public class Player : Character
    {
        public double DashCool { get; set; }
        public string PlayerType { get; set; }

        public Player(Vector2 Position) : base(Position)
        {
            this.DashCool = 120;
            this.Speed = 5;
            this.AttackSpeed = 25;
            this.AttackCooldown = AttackSpeed;
            this.ProjectileSpeed = 4;

            this.Texture = TextureManager.GetTexture("player");
            this.Centre = new Vector2(Texture.Width / 2, Texture.Height / 2);
            this.Size = new Vector2(Texture.Width, Texture.Height);
        }

        public void Move(KeyboardState KB, GamePadState GP, List<Enemy> enemyList, Map map)
        {
            Vector2 OldLocation = Position;
            Vector2 translation = CalcMove(KB, GP);
            Vector2 NewLocation = OldLocation + translation;

            Rectangle newRect = RectCalc(NewLocation, Size);

            bool intersects = false;
            foreach (Enemy E in enemyList)
            {
                if (E.Rectangle.Intersects(newRect))
                {
                    intersects = true;
                    break;
                }
            }
            if (intersects == false)
            {
                for (int i = Convert.ToInt32((this.Position.X / 32) - 20); i <= this.Position.X / 32 + 20; i++)
                {
                    for (int j = Convert.ToInt32((this.Position.Y / 32) - 20); j <= this.Position.Y / 32 + 20; j++)
                    {
                        if (i >= 0 && i< map.WidthNodes && j >= 0 && j < map.HeightNodes)
                        {
                            Node N = map.Grid[i, j];
                            if (N.Rectangle.Intersects(newRect) && N.Walkable == false)
                            {
                                intersects = true;
                                break;
                            }
                        }
                    }
                }
            }
            if (!map.RectanglePixel.Contains(newRect))
            {
                intersects = true;
            }
            if (intersects == false)
            {
                this.Position = NewLocation;
            }

            updateRect();
        }

        public Vector2 CalcMove(KeyboardState KB, GamePadState GP)
        {
            Vector2 transformation = new Vector2(0, 0);
            if (KB.IsKeyDown(Keys.A))
            {
                transformation.X -= 1;
            }
            if (KB.IsKeyDown(Keys.D))
            {
                transformation.X += 1;
            }
            if (KB.IsKeyDown(Keys.W))
            {
                transformation.Y -= 1;
            }
            if (KB.IsKeyDown(Keys.S))
            {
                transformation.Y += 1;
            }
            if (transformation != Vector2.Zero)
            {
                transformation.Normalize();
            }
            transformation = transformation * Speed;

            return transformation;
        }

        public void Attack(KeyboardState KB, GamePadState GP)
        {
            
            if (AttackCooldown == 0)
            {
                if (KB.IsKeyDown(Keys.Down))
                {
                    ProjectileManager.Projectiles.Add(new Projectile(Position, 0, ProjectileSpeed, Range));
                }
                if (KB.IsKeyDown(Keys.Up))
                {
                    ProjectileManager.Projectiles.Add(new Projectile(Position, 0, -ProjectileSpeed, Range));
                }
                if (KB.IsKeyDown(Keys.Left))
                {
                    ProjectileManager.Projectiles.Add(new Projectile(Position, -ProjectileSpeed, 0, Range));
                }
                if (KB.IsKeyDown(Keys.Right))
                {
                    ProjectileManager.Projectiles.Add(new Projectile(Position, ProjectileSpeed, 0, Range));
                }
                this.AttackCooldown = AttackSpeed;
            }
            else
            {
                AttackCooldown -= 1;
            }
        }
    }
}

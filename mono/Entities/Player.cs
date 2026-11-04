using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace mono.Entities
{
    public class Player : Character
    {
        public double DashCool { get; set; }
        public string PlayerType { get; set; }
        public bool Dead { get; private set; } = false;

        public Player(Vector2 Position) : base(Position)
        {
            this.DashCool = 120;
            this.Speed = 5;
            this.AttackSpeed = 25;
            this.AttackCooldown = AttackSpeed;
            this.ProjectileSpeed = 7;

            this.MaxHP = 3;
            this.HP = MaxHP;

            this.Texture = TM.GetTexture("player");
            this.Centre = new Vector2(Texture.Width / 2, Texture.Height / 2);
            this.Size = new Vector2(Texture.Width, Texture.Height);
        }

        public void Move(List<Enemy> enemyList, Map map)
        {
            Vector2 oldLocation = Position;
            Vector2 translation = CalcMove();
            Vector2 newLocation = oldLocation + translation;

            Rectangle newRect = RectCalc(newLocation, Size);

            bool intersects = false;
            bool slow = false;
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
                List<Node> nodes = new List<Node>();
                Node n = map.Grid[(int)newLocation.X / 32, (int)newLocation.Y / 32];
                nodes.Add(n);
                foreach (Node a in n.Neigbour)
                {
                    nodes.Add(a);
                }
                foreach (Node a in nodes)
                {
                    if (newRect.Intersects(a.Rectangle) && a.TileType == TileType.wall)
                    {
                        intersects = true;
                        break;
                    }
                    else if (newRect.Intersects(a.Rectangle) && a.TileType == TileType.water)
                    {
                        slow = true;
                    }
                }
            }
            if (!map.RectanglePixel.Contains(newRect))
            {
                intersects = true;
            }
            if (intersects == false)
            {
                if (slow == true)
                {
                    translation = translation * 0.8f;
                    newLocation = oldLocation + translation;
                }
                this.Position = newLocation;
            }

            updateRect();
        }

        public Vector2 CalcMove()
        {
            Vector2 transformation = new Vector2(0, 0);
            if (IM.IsKeyDown(Keys.A))
            {
                transformation.X -= 1;
            }
            if (IM.IsKeyDown(Keys.D))
            {
                transformation.X += 1;
            }
            if (IM.IsKeyDown(Keys.W))
            {
                transformation.Y -= 1;
            }
            if (IM.IsKeyDown(Keys.S))
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

        public void Attack()
        {
            if (AttackCooldown == 0)
            {
                if (IM.IsKeyDown(Keys.Down))
                {
                    PM.AddProjectile(Position, 0, ProjectileSpeed, Range, Owner.player);
                    this.AttackCooldown = AttackSpeed;
                }
                else if (IM.IsKeyDown(Keys.Up))
                {
                    PM.AddProjectile(Position, 0, -ProjectileSpeed, Range, Owner.player);
                    this.AttackCooldown = AttackSpeed;
                }
                else if (IM.IsKeyDown(Keys.Left))
                {
                    PM.AddProjectile(Position, -ProjectileSpeed, 0, Range, Owner.player);
                    this.AttackCooldown = AttackSpeed;
                }
                else if (IM.IsKeyDown(Keys.Right))
                {
                    PM.AddProjectile(Position, ProjectileSpeed, 0, Range, Owner.player);
                    this.AttackCooldown = AttackSpeed;
                }
            }
            else
            {
                AttackCooldown -= 1;
            }
        }


        public override void DamageEntity()
        {
            HP -= 1;
            if (HP <= 0)
            {
                Dead = true;
            }
        }
    }
}

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
        public bool Dead { get; private set; } = false;

        public Player(Vector2 position, CharacterData data) :
                 base(position, data)
        {
            this.DashCool = 120;
            
            

            
            
        }

        public void Move(Map map)
        {
            List<Node> nodes = new List<Node>();
            Node n = map.Grid[(int)Position.X / 32, (int)Position.Y / 32];
            nodes.Add(n);
            foreach (Node a in n.Neigbour)
            {
                nodes.Add(a);
                
            }
            
            Vector2 translation = CalcMove();

            Rectangle newRect = RectCalc(newLocation, Size);

            bool intersects = false;
            bool slow = false;
            foreach (Enemy E in CM.EnemyList)
            {
                if (E.Rectangle.Intersects(newRect))
                {
                    intersects = true;
                    break;
                }
            }
            if (intersects == false)
            {
                
                foreach (Node a in nodes)
                {
                    if (newRect.Intersects(a.Rectangle) && a.TileType == TileType.wall)
                    {
                        intersects = true;
                        break;
                    }
                    
                }
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

            updateRect(map);
            if (this.CurrentNode.TileType == TileType.water)
            {
                Effects[CharacterEffect.Slow] = true;
            }
        }

        public Vector2 CalcMove()
        {
            Vector2 transformation = new Vector2(0, 0);
            if (IM.KeyDown(Keys.A))
            {
                transformation.X -= 1;
            }
            if (IM.KeyDown(Keys.D))
            {
                transformation.X += 1;
            }
            if (IM.KeyDown(Keys.W))
            {
                transformation.Y -= 1;
            }
            if (IM.KeyDown(Keys.S))
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

        public bool Collides(Rectangle R, Map map)
        {
            if (!map.RectanglePixel.Contains(R))
            {
                return true;
            }




            return false;
        }

        public override void Attack()
        {
            if (AttackCooldown == 0)
            {
                if (IM.KeyDown(Keys.Down))
                {
                    PM.AddProjectile(Position, Damage, 1, 0, ProjectileSpeed, Range, Owner.player);
                    this.AttackCooldown = AttackSpeed;
                }
                else if (IM.KeyDown(Keys.Up))
                {
                    PM.AddProjectile(Position, Damage, 1, 0, -ProjectileSpeed, Range, Owner.player);
                    this.AttackCooldown = AttackSpeed;
                }
                else if (IM.KeyDown(Keys.Left))
                {
                    PM.AddProjectile(Position, Damage, 1, -ProjectileSpeed, 0, Range, Owner.player);
                    this.AttackCooldown = AttackSpeed;
                }
                else if (IM.KeyDown(Keys.Right))
                {
                    PM.AddProjectile(Position, Damage, 1, ProjectileSpeed, 0, Range, Owner.player);
                    this.AttackCooldown = AttackSpeed;
                }
            }
            else
            {
                AttackCooldown -= 1;
            }
        }


        public override void DamageEntity(int damage, Vector2 p)
        {
            HP -= damage;
            if (HP <= 0)
            {
                Dead = true;
            }
        }
    }
}
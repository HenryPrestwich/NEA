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

        public Player(Vector2 position, CharacterData data, Map map) :
                 base(position, data, map)
        {
            this.DashCool = 120;





        }

        public override void Move(Map map)
        {
            Vector2 translation = CalcMove();
            
            if (Collides(translation, map) == false)
            {
                this.Position += translation;
            }
            else
            {
                if (ApproachWall(translation, map, 1) == false)
                {
                    if (Collides(new Vector2(translation.X, 0), map) == true)
                    {
                        this.Position += new Vector2(translation.X, 0);
                    }
                    else if (Collides(new Vector2(0, translation.Y), map) == true)
                    {
                        this.Position += new Vector2(0, translation.Y);
                    }
                }
            }




            updateRect(map);
            if (this.CurrentNode.TileType == TileType.water)
            {
                Effects[CharacterEffect.Slow] = true;
            }
            if (this.CurrentNode.TileType == TileType.floor)
            {
                Effects[CharacterEffect.Slow] = false;
            }
        }

        public bool ApproachWall(Vector2 translation, Map map, float multiplier)
        {
            if (multiplier <= 0)
            {
                return false;
            }
            else
            {
                if (Collides(translation * multiplier, map) == false)
                {
                    this.Position += translation * multiplier;
                    return true;
                }
                else
                {
                    return ApproachWall(translation, map, multiplier - 0.1f);
                }
            }
        }



        public Vector2 CalcMove()
        {
            Vector2 translation = new Vector2(0, 0);
            if (IM.KeyDown(Keys.A))
            {
                translation.X -= 1;
            }
            if (IM.KeyDown(Keys.D))
            {
                translation.X += 1;
            }
            if (IM.KeyDown(Keys.W))
            {
                translation.Y -= 1;
            }
            if (IM.KeyDown(Keys.S))
            {
                translation.Y += 1;
            }
            if (translation != Vector2.Zero)
            {
                translation.Normalize();
            }
            translation = translation * Speed;

            return translation;
        }

        public bool Collides(Vector2 translation, Map map)
        {
            Vector2 newPosition = this.Position + translation;
            Rectangle R = RectCalc(newPosition, Size);

            if (!map.RectanglePixel.Contains(R))
            {
                return true;
            }

            List<Node> nodes = new List<Node>();
            Node n = CM.Player.CurrentNode;
            nodes.Add(n);
            foreach (Node a in n.Neigbour)
            {
                nodes.Add(a);
            }
            foreach (Enemy E in CM.EnemyList)
            {
                if (E.Rectangle.Intersects(R))
                {
                    return true;
                }
            }
            foreach (Node a in nodes)
            {
                if (R.Intersects(a.Rectangle) && a.TileType == TileType.wall)
                {
                    return true;
                }

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
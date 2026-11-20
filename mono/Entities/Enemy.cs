using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace mono.Entities
{
    public class Enemy : Character
    {
        public EnemyState state { get; set; }
        public int Vision { get; set; }



        public Queue<Node> path = new Queue<Node>();
        public Node NextNode { get; private set; }

        public Enemy(Vector2 position, CharacterData data) :
                base(position, data)
        {
            this.state = EnemyState.IDLE;
            this.Vision = data.Vision;
        }

        public void SetState()
        {
            switch (state)
            {
                case EnemyState.IDLE:
                    if (path != null && path.Count < Vision)
                    {
                        this.state = EnemyState.CHASE;
                    }
                    break;
                case EnemyState.CHASE:
                    if (path != null && path.Count < Range)
                    {
                        this.state = EnemyState.ATTACK;
                    }
                    break;
                case EnemyState.ATTACK:
                    
                        break;

            }


        }


        public void SetPath(Map map)
        {
            path = AStar.ASTAR(this.Position, map);
        }
        public void Move()
        {
            if (path != null && path.Count > 0)
            {
                Node next = path.Peek();

                Vector2 OldLocation = Position;
                Vector2 NewLocation = Position + CalcMove(next);


                Rectangle ERect = RectCalc(NewLocation, Size);

                bool collides = false;

                foreach (Enemy e in CM.EnemyList)
                {
                    if (ERect.Intersects(e.Rectangle) && this != e)
                    {
                        collides = true; break;
                    }
                }

                if (!ERect.Intersects(CM.Player.Rectangle) && collides == false)
                {
                    Vector2 distanceV = next.Position - this.Position;
                    double distanceD = Math.Sqrt(Math.Pow(distanceV.X, 2) + Math.Pow(distanceV.Y, 2));
                    if (distanceD < this.Speed)
                    {
                        path.Dequeue();
                        this.Position = next.Position;
                    }
                    else
                    {
                        this.Position = NewLocation;
                    }
                }
            }
        }
        public Vector2 CalcMove(Node Next)
        {
            Vector2 transformation = new Vector2(0, 0);

            if (Next.Position.X < this.Position.X)
            {
                transformation.X -= 1;
            }
            if (Next.Position.X > this.Position.X)
            {
                transformation.X += 1;
            }
            if (Next.Position.Y < this.Position.Y)
            {
                transformation.Y -= 1;
            }
            if (Next.Position.Y > this.Position.Y)
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

        public void DrawPath(SpriteBatch spriteBatch)
        {
            if (path != null)
            {
                foreach (Node n in path)
                {
                    spriteBatch.Draw(TM.GetTexture("pixel"), new Rectangle(Convert.ToInt32(n.Rectangle.X), Convert.ToInt32(n.Rectangle.Y), 32, 1), Color.Red);

                    spriteBatch.Draw(TM.GetTexture("pixel"), new Rectangle(Convert.ToInt32(n.Rectangle.X), Convert.ToInt32(n.Rectangle.Y) + 31, 32, 1), Color.Red);

                    spriteBatch.Draw(TM.GetTexture("pixel"), new Rectangle(Convert.ToInt32(n.Rectangle.X), Convert.ToInt32(n.Rectangle.Y), 1, 32), Color.Red);

                    spriteBatch.Draw(TM.GetTexture("pixel"), new Rectangle(Convert.ToInt32(n.Rectangle.X) + 31, Convert.ToInt32(n.Rectangle.Y), 1, 32), Color.Red);
                }
            }
        }

        public override void Attack()
        {

        }

        public override void DamageEntity(int damage)
        {
            HP -= damage;
            if (HP < 0)
            {
                state = EnemyState.DEAD;
            }
        }
    }

    public enum EnemyState
    {
        IDLE = 0,
        CHASE,
        ATTACK,
        DEAD
    }
}
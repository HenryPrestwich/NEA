using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace mono.Entities
{
    public class Enemy : Character
    {
        public int state { get; set; }
        public Queue<Node> path = new Queue<Node>();
        public Node NextNode { get; private set; }

        public Enemy(Vector2 Position) : base(Position)
        {
            this.Speed = 3;
            this.state = EnemyState.IDLE;

            this.Texture = TextureManager.GetTexture("enemy");



            Centre = new Vector2(Texture.Width / 2, Texture.Height / 2);
            this.Size = new Vector2(Texture.Width, Texture.Height);
        }

        public void CheckState(Player p)
        {
            if (state == EnemyState.IDLE)
            {

            }
        }

        public void SetPath(Player player, Map map)
        {
            path = AStar.ASTAR(this.Position, player.Position, map);
        }
        public void Move(Player p, List<Enemy> enemyL)
        {
            if (path != null && path.Count > 0)
            {
                Node next = path.Peek();

                Vector2 OldLocation = Position;
                Vector2 NewLocation = Position + CalcMove(next);


                Rectangle ERect = RectCalc(NewLocation, Size);

                bool collides = false;

                foreach (Enemy e in enemyL)
                {
                    if (ERect.Intersects(e.Rectangle) && this != e)
                    {
                        collides = true; break;
                    }
                }

                if (!ERect.Intersects(p.Rectangle) && collides == false)
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

            Node next = Next;

            Vector2 transformation = new Vector2(0, 0);

            if (next.Position.X < this.Position.X)
            {
                transformation.X -= 1;
            }
            if (next.Position.X > this.Position.X)
            {
                transformation.X += 1;
            }
            if (next.Position.Y < this.Position.Y)
            {
                transformation.Y -= 1;
            }
            if (next.Position.Y > this.Position.Y)
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
                    spriteBatch.Draw(TextureManager.GetTexture("pixel"), new Rectangle(Convert.ToInt32(n.Rectangle.X), Convert.ToInt32(n.Rectangle.Y), 32, 1), Color.Red);

                    spriteBatch.Draw(TextureManager.GetTexture("pixel"), new Rectangle(Convert.ToInt32(n.Rectangle.X), Convert.ToInt32(n.Rectangle.Y) + 31, 32, 1), Color.Red);

                    spriteBatch.Draw(TextureManager.GetTexture("pixel"), new Rectangle(Convert.ToInt32(n.Rectangle.X), Convert.ToInt32(n.Rectangle.Y), 1, 32), Color.Red);

                    spriteBatch.Draw(TextureManager.GetTexture("pixel"), new Rectangle(Convert.ToInt32(n.Rectangle.X) + 31, Convert.ToInt32(n.Rectangle.Y), 1, 32), Color.Red);
                }
            }
        }
    }

    public static class EnemyState
    {
        public const int IDLE = 0;
    }
}
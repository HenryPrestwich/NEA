using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace mono.Entities
{
    public class Enemy : Character
    {
        public string state;
        public Queue<Node> path = new Queue<Node>();
        public Node NextNode { get; private set; }

        public Enemy(Texture2D texture, Vector2 Position) : base(texture, Position)
        {
            state = null;
            this.Speed = 3;
        }

        public void CheckState()
        {

        }

        public void SetPath(Player player, Graph graph)
        {
            path = AStar.ASTAR(this.Position, player.Position, graph);
        }
        public override void Move(Player p)
        {
            if (path != null && path.Count > 0)
            {
                Node next = path.Peek();

                Vector2 OldLocation = Position;
                Vector2 NewLocation = Position + CalcMove(next);

                Rectangle PRect = p.Rectangle;

                Rectangle ERect = RectCalc(NewLocation, Size);

                if (!ERect.Intersects(PRect))
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
        public void DrawPath(SpriteBatch spriteBatch, Texture2D pixel)
        {
            if (path != null)
            {
                foreach (Node n in path)
                {
                    spriteBatch.Draw(pixel, new Rectangle(Convert.ToInt32(n.Rectangle.X), Convert.ToInt32(n.Rectangle.Y), 32, 1), Color.Red);

                    spriteBatch.Draw(pixel, new Rectangle(Convert.ToInt32(n.Rectangle.X), Convert.ToInt32(n.Rectangle.Y) + 31, 32, 1), Color.Red);

                    spriteBatch.Draw(pixel, new Rectangle(Convert.ToInt32(n.Rectangle.X), Convert.ToInt32(n.Rectangle.Y), 1, 32), Color.Red);

                    spriteBatch.Draw(pixel, new Rectangle(Convert.ToInt32(n.Rectangle.X) + 31, Convert.ToInt32(n.Rectangle.Y), 1, 32), Color.Red);
                }
            }
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mono.Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace mono.Entities
{
    public class Enemy : Character
    {
        public string state;
        public Queue<Node> path = new Queue<Node>();
        public Node NextNode { get; private set; }
        
        public Enemy(Texture2D texture, Vector2 Position) : base(texture,  Position)
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
            Vector2 OldLocation = Position;
            Vector2 NewLocation = Position + CalcMove();

            Rectangle PRect = p.Rectangle;

            Rectangle ERect = RectCalc(Position, Size);

            if (!ERect.Intersects(PRect))
            {
                this.Position = NewLocation;   
            }
        }
        public Vector2 CalcMove()
        {
            if (path != null && path.Count != 0)
            {
                Node next = path.Peek();
                
                Vector2 transformation = new Vector2(0, 0);

                Vector2 distanceV = next.Position - this.Position;
                double distanceD = Math.Sqrt(Math.Pow(distanceV.X, 2) +  Math.Pow(distanceV.Y, 2));
                if (distanceD < this.Speed)
                {
                    transformation = distanceV;
                    path.Dequeue();
                    return transformation;
                }

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
            return new Vector2(this.Position.X, this.Position.Y);
        }
        public void DrawPath(SpriteBatch spriteBatch, Texture2D pixel)
        {
            if (path != null)
            {
                foreach (Node n in path)
                {
                    spriteBatch.Draw(pixel, new Rectangle(Convert.ToInt32(n.Position.X), Convert.ToInt32(n.Position.Y), 32, 1), Color.Red);

                    spriteBatch.Draw(pixel, new Rectangle(Convert.ToInt32(n.Position.X), Convert.ToInt32(n.Position.Y) + 31, 32, 1), Color.Red);

                    spriteBatch.Draw(pixel, new Rectangle(Convert.ToInt32(n.Position.X), Convert.ToInt32(n.Position.Y), 1, 32), Color.Red);

                    spriteBatch.Draw(pixel, new Rectangle(Convert.ToInt32(n.Position.X) + 31, Convert.ToInt32(n.Position.Y), 1, 32), Color.Red);
                }
            }
        }
    }
}
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
        
        //ublic bool 

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
          
            if (path.Count == 0)
            {
                path = AStar.ASTAR(this.Position, player.Position, graph);

            }
             
        }
        public override void Move(Player p)
        {
            Vector2 OldLocation = Position;
            Vector2 NewLocation = CalcMove();

            Rectangle PRect = new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), this.Texture.Width, this.Texture.Height);

            Rectangle ERect = new Rectangle(Convert.ToInt32(p.Position.X), Convert.ToInt32(p.Position.Y), p.Texture.Width, p.Texture.Height);

            if (!ERect.Intersects(PRect))
            {
                this.Position = NewLocation;
            }
        }
        public Vector2 CalcMove()
        {
            if (path != null && path.Count != 0)
            {
                Node next = path.Dequeue();
                if (next.Position.X > this.Position.X)
                {
                    return new Vector2(this.Position.X + Speed, this.Position.Y);
                }
                if (next.Position.Y > this.Position.Y)
                {
                    return new Vector2(this.Position.X, this.Position.Y + Speed);
                }
                if (next.Position.X < this.Position.X)
                {
                    return new Vector2(this.Position.X - Speed, this.Position.Y);
                }
                if (next.Position.Y < this.Position.Y)
                {
                    return new Vector2(this.Position.X, this.Position.Y - Speed);
                }
            }
            return new Vector2(this.Position.X, this.Position.Y);
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mono.Main;
using System;
using System.Collections.Generic;
using System.IO;


namespace mono.Entities
{
    public class Enemy : Character
    {
        public string state;
        public Queue<Node> path;
        public Enemy(Texture2D texture) : base(texture)
        {
            state = null;
        }
        
        public void CheckState()
        {
            
        }

        public void SetPath(Player player, Graph graph)
        {
            if (path.)
            path = AStar.ASTAR(this.Position, player.Position, graph);
             
        }

        public override void Move()
        {
            if (path != null && path.Count != 0)
            {
                Node next = path.Dequeue();
                if (next.Position.X > this.Position.X)
                {
                    this.Position = new Vector2(this.Position.X + 5, this.Position.Y);
                }
                if (next.Position.Y > this.Position.Y)
                {
                    this.Position = new Vector2(this.Position.X, this.Position.Y + 5);
                }
                if (next.Position.X < this.Position.X)
                {
                    this.Position = new Vector2(this.Position.X - 5, this.Position.Y);
                }
                if (next.Position.Y < this.Position.Y)
                {
                    this.Position = new Vector2(this.Position.X, this.Position.Y - 5);
                }
            }
        }
    }
}
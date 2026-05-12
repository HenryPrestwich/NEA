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
            path = AStar.ASTAR(this.Position, player.Position, graph);
             
        }

        public override void Move()
        {
            if (path.Count != 0)
            {
                this.Position = path.Dequeue().Position;
            }
        }
    }
}
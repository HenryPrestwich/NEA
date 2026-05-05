using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mono.Main;
using System;
using System.Collections.Generic;
using System.IO;


namespace mono.Entities
{
    internal class Enemy : Character
    {
        string state;
        Queue<Node> path;
        public Enemy(Texture2D texture) : base(texture)
        {
            state = null;
        }
        
        public void CheckState()
        {
            
        }

        public void SetPath(Player player)
        {
            path = AStar.ASTAR(this.NodePosition, player.NodePosition);
             
        }

        public override void Move()
        {

        }
    }
}
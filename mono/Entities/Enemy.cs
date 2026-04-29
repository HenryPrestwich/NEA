using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;


namespace mono.Entities
{
    internal class Enemy : Character
    {
        string state;

        public Enemy(Texture2D texture) : base(texture)
        {
            state = null;
        }
        
        public void CheckState()
        {
            
        }

        public override void Move()
        {

        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Timers;

namespace mono.Entities
{
    public class Player : Character
    {
        
        public double DashCool {  get; set; }

        

        public Player(Texture2D texture) : base(texture)
        {
            DashCool = 120;
        }

     
        public override void Move(KeyboardState KB, GamePadState GP)
        {
            if (KB.IsKeyDown(Keys.A))
            {
                Position = new Vector2(Position.X - 5, Position.Y);
            }
            if (KB.IsKeyDown(Keys.D))
            {
                Position = new Vector2(Position.X + 5, Position.Y);
            }
            if (KB.IsKeyDown(Keys.W))
            {
                Position = new Vector2(Position.X, Position.Y - 5);
            }
            if (KB.IsKeyDown(Keys.S))
            {
                Position = new Vector2(Position.X, Position.Y + 5);
            }

            Dash(KB, GP);
        }

        public void Dash(KeyboardState KB, GamePadState GP)
        {
            if (KB.IsKeyDown(Keys.Space))
            {
                if (DashCool == 0)
                {
                    if (KB.IsKeyDown(Keys.A))
                    {
                        Position = new Vector2(Position.X - 75, Position.Y);
                    }
                    if (KB.IsKeyDown(Keys.D))
                    {
                        Position = new Vector2(Position.X + 75, Position.Y);
                    }
                    if (KB.IsKeyDown(Keys.W))
                    {
                        Position = new Vector2(Position.X, Position.Y - 75);
                    }
                    if (KB.IsKeyDown(Keys.S))
                    {
                        Position = new Vector2(Position.X, Position.Y + 75);
                    }
                    DashCool += 120;
                }
            }
            if (DashCool > 0)
            {
                DashCool -= 1;
            }
        }
    }
}

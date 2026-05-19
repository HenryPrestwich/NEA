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

        

        public Player(Texture2D texture, Vector2 Position) : base(texture, Position)
        {
            DashCool = 120;
        }

        public override void Move(KeyboardState KB, GamePadState GP, List<Character> charlist)
        {
            Vector2 OldLocation = Position;
            Vector2 NewLocation = CalcMove(KB, GP);

            Rectangle PRect = new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), this.Texture.Width, this.Texture.Height);
            foreach (Enemy E in charlist)
            {
                Rectangle ERect = new Rectangle(Convert.ToInt32(E.Position.X), Convert.ToInt32(E.Position.Y), E.Texture.Width, E.Texture.Height);
                if (!ERect.Intersects(PRect))
                {
                    this.Position = NewLocation;
                }
            }
        }
     
        public Vector2 CalcMove(KeyboardState KB, GamePadState GP)
        {
            if (KB.IsKeyDown(Keys.A))
            {
                return new Vector2(Position.X - 5, Position.Y);
            }
            if (KB.IsKeyDown(Keys.D))
            {
                return new Vector2(Position.X + 5, Position.Y);
            }
            if (KB.IsKeyDown(Keys.W))
            {
                return new Vector2(Position.X, Position.Y - 5);
            }
            if (KB.IsKeyDown(Keys.S))
            {
                return new Vector2(Position.X, Position.Y + 5);
            }
            return new Vector2(Position.X, Position.Y);
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

        public void Collision(Enemy enemy)
        {
            //if (
            //    enemy.Position.X > this.Position.X &&
            //    enemy.Position .Y 
            //    )
            //{

            //}
        }
    }
}

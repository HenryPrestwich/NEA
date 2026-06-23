using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Vector2 translation = CalcMove(KB, GP);

            List<Enemy> enemylist = charlist.OfType<Enemy>().ToList();

            bool intersects = false;
            foreach (Enemy E in enemylist)
            {
                    if (E.Rectangle.Intersects(this.Rectangle))
                    {
                    intersects = true;
                    break;
                    }   
            }
            if (intersects == false)
            {
                this.Position += translation;
            }
        }
     
        public Vector2 CalcMove(KeyboardState KB, GamePadState GP)
        {
            Vector2 transformation = new Vector2(0, 0);
            if (KB.IsKeyDown(Keys.A))
            {
                transformation = new Vector2(-5, 0);
            }
            else if (KB.IsKeyDown(Keys.D))
            {
                transformation = new Vector2(5, 0);
            }
            else if (KB.IsKeyDown(Keys.W))
            {
                transformation = new Vector2(0, -5);
            }
            else if (KB.IsKeyDown(Keys.S))
            {
                transformation = new Vector2(0, 5);
            }
            if (transformation != Vector2.Zero)
            {
                transformation.Normalize();
            } 

            return transformation;
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

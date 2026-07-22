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

        public override void Move(KeyboardState KB, GamePadState GP, List<Character> charlist, Graph graph)
        {
            Vector2 OldLocation = Position;
            Vector2 translation = CalcMove(KB, GP);
            Vector2 NewLocation = OldLocation + translation;

            List<Enemy> enemylist = charlist.OfType<Enemy>().ToList();

            Rectangle newRect = RectCalc(NewLocation, Size);

            bool intersects = false;
            foreach (Enemy E in enemylist)
            {
                    if (E.Rectangle.Intersects(newRect))
                    {
                    intersects = true;
                    break;
                    }   
            }
            foreach (Node N in graph.Grid)
            {
                if(N.Rectangle.Intersects(newRect) && N.Walkable == false) 
                {
                    intersects = true; 
                    break; 
                }
            }
            if (intersects == false)
            {
                this.Position = NewLocation;
            }
        }
     
        public Vector2 CalcMove(KeyboardState KB, GamePadState GP)
        {
            Vector2 transformation = new Vector2(0, 0);
            if (KB.IsKeyDown(Keys.A))
            {
                transformation.X -= 1;
            }
            if (KB.IsKeyDown(Keys.D))
            {
                transformation.X += 1;
            }
            if (KB.IsKeyDown(Keys.W))
            {
                transformation.Y -= 1;
            }
            if (KB.IsKeyDown(Keys.S))
            {
                transformation.Y += 1;
            }
            if (transformation != Vector2.Zero)
            {
                transformation.Normalize();
            }
            transformation = transformation * 5; //replace 5 with player.speed when it exists

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

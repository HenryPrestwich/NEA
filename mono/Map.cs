using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mono.Main;
using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Windows.Forms;


namespace mono
{
    public class Map
    {
        public Texture2D grass;
        public Texture2D wall;

        public Rectangle Rectangle {  get; set; }
        

        public Node[,] Grid { get; set; }
        public int WidthNodes {  get; set; }
        public int HeightNodes { get; set; }

        public Map(int height, int width, Texture2D grass, Texture2D wall)
        {
            this.grass = grass;
            this.wall = wall;

            
            this.WidthNodes = width / 32;
            this.HeightNodes = height / 32;
            
            

            this.Rectangle = new Rectangle(0, 0, WidthNodes * 32, HeightNodes * 32);

            Grid = new Node[WidthNodes, HeightNodes];

            for (int x = 0; x < WidthNodes; x++)
            {
                for (int y = 0; y < HeightNodes; y++)
                {
                    Node n = new Node(x, y, 0); 
                    Grid [x, y] = n;
                }
            }

            for (int x = 0;x < WidthNodes; x++)
            {
                for (int y = 0; y < HeightNodes; y++)
                {
                    if (x > 0)
                    {
                        Grid[x, y].Neigbour.Add(Grid[x - 1, y]);
                    }
                    if (y > 0)
                    {
                        Grid[x, y].Neigbour.Add(Grid[x, y - 1]);
                    }
                    if (x > 0 && y >0)
                    {
                        Grid[x, y].Neigbour.Add(Grid[x - 1, y - 1]);

                    }
                    if (x < WidthNodes -1)
                    {
                        Grid[x, y].Neigbour.Add(Grid[x + 1, y]);
                    }
                    if (y < HeightNodes - 1)
                    {
                        Grid[x, y].Neigbour.Add(Grid[x, y + 1]);
                    }
                    if (x < WidthNodes - 1 && y < HeightNodes - 1)
                    {
                        Grid[x, y].Neigbour.Add(Grid[x + 1, y + 1]);
                    }
                    if (x < WidthNodes - 1 && y > 0)
                    {
                        Grid[x, y].Neigbour.Add(Grid[x + 1, y - 1]);
                    }
                    if (x > 0 && y < HeightNodes - 1)
                    {
                        Grid[x, y].Neigbour.Add(Grid[x - 1, y + 1]);
                    }
                }
            }  
        }

        public void DrawMap(SpriteBatch spriteBatch)
        {
            foreach (Node n in Grid)
            {
                if (n.Walkable == true)
                {
                    spriteBatch.Draw(grass, n.Position, null, Color.White, 0f, n.Centre, 1f, SpriteEffects.None, Layers.Background);
                }
                else
                {
                    spriteBatch.Draw(wall, n.Position, null, Color.White, 0f, n.Centre, 1f, SpriteEffects.None, Layers.Background);
                }
            }
        }
    }

    public class Node          
    {
        public Vector2 GridLocation { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Centre {  get; set; }
        public Rectangle Rectangle { get; set; }
        public Vector2 Size { get; set; }
        public List<Node> Neigbour  { get; set; }
        public bool Walkable { get; set; }
        public int TileType { get; set; }

        public Node(int x, int y, int tileType)
        {
            this.Size = new Vector2(32, 32);
            this.GridLocation = new Vector2(x, y);
            this.Position = new Vector2(x * 32, y * 32);
            this.Centre = new Vector2(16, 16);
            

            this.Rectangle = new Rectangle(Convert.ToInt32(Position.X - Size.X / 2), Convert.ToInt32(Position.Y - Size.Y / 2), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y));

            this.Neigbour = new List<Node>();
            this.TileType = tileType;
            Random rand = new Random();
            if (rand.Next(1, 1) ==0)
            {
                Walkable = false;
            }
            else
            {
                Walkable = true;
            }

        }
    }

    public class Room
    {
        public Vector2 Position { get; set; }
        public Rectangle Size { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }


        private Room(Map Map)
        {
            Random rand = new Random();
            Position = new Vector2(rand.Next(0, Map.WidthNodes), rand.Next(0, Map.HeightNodes)); //THIS MUST ALIGN WITH NODES NOT PIXELS!!!
            Width = rand.Next(12, 25);
            Height = rand.Next(12, 25);
            
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace mono
{
    

    public class Graph
    {
        public Texture2D grass;
        public Texture2D wall;

        public Node[,] Grid { get; set; }
        int WidthNodes;
        int HeightNodes;

        public Graph(int height, int width, Texture2D grass, Texture2D wall)
        {
            this.grass = grass;
            this.wall = wall;


            this.WidthNodes = width / 32;
            this.HeightNodes = height / 32;

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

            //for (int x = 0; x < 20; x++)
            //{
            //    SetWalk();
            //}
        }

        public void DrawMap(SpriteBatch spriteBatch)
        {
            foreach (Node n in Grid)
            {
                if (n.Walkable == true)
                {
                    spriteBatch.Draw(grass, n.Position, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, Layers.Background);
                }
                else
                {
                    spriteBatch.Draw(wall, n.Position, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, Layers.Background);
                }
            }
        }

        //public void SetWalk()
        //{
        //    foreach (Node n in Grid)
        //    {
        //        int walkableN = 0;
        //        foreach (Node o in n.Neigbour)
        //        {
        //            if (o.Walkable == true)
        //            {
        //                walkableN++;
        //            }
        //        }
        //        if (walkableN > 6)
        //        {
        //            n.Walkable = true;
        //        }
        //        else if (walkableN < 4)
        //        {
        //            n.Walkable = false;
        //        }
        //    }
        //}
    }

    public class Node          
    {
        public Vector2 GridLocation;
        public Vector2 Position;
        public Rectangle Rectangle;
        public List<Node> Neigbour;
        public bool Walkable;
        public int TileType;

        public Node(int x, int y, int tileType)
        {
            this.GridLocation = new Vector2(x, y);
            this.Position = new Vector2(x * 32, y * 32);

            this.Rectangle = new Rectangle(Convert.ToInt32(Position.X),Convert.ToInt32(Position.Y), 32, 32);

            this.Neigbour = new List<Node>();
            this.TileType = tileType;
            Random rand = new Random();
            if (rand.Next(0, 10) ==0)
            {
                Walkable = false;
            }
            else
            {
                Walkable = true;
            }

        }
    }
}

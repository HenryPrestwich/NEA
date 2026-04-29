using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Windows.Forms;

namespace mono
{
    internal class Map
    {
        public Texture2D texture;

        public Map(Texture2D texture)
        {
            this.texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(0, 0), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, Layers.Map);
        }
    }

    class Graph
    {
        public Node[,] grid;
        int WidthNodes;
        int HeightNodes;

        public Graph(int height, int width)
        {

            this.WidthNodes = width / 32;
            this.HeightNodes = height / 32;

            grid = new Node[WidthNodes, HeightNodes];

            for (int x = 0; x < WidthNodes; x++)
            {
                for (int y = 0; y < HeightNodes; y++)
                {
                    Node n = new Node(x, y, 0); //Add tile type here
                    grid [x, y] = n;
                }
            }

            for (int x = 0;x < WidthNodes; x++)
            {
                for (int y = 0; y < HeightNodes; y++)
                {
                    if (x > 0)
                    {
                        grid[x, y].Neigbour.Add(grid[x - 1, y]);
                    }
                    if (y > 0)
                    {
                        grid[x, y].Neigbour.Add(grid[x, y - 1]);
                    }
                    if (x > 0 && y >0)
                    {
                        grid[x, y].Neigbour.Add(grid[x - 1, y - 1]);

                    }
                    if (x < WidthNodes -1)
                    {
                        grid[x, y].Neigbour.Add(grid[x + 1, y]);
                    }
                    if (y < HeightNodes - 1)
                    {
                        grid[x, y].Neigbour.Add(grid[x, y + 1]);
                    }
                    if (x < WidthNodes - 1 && y < HeightNodes - 1)
                    {
                        grid[x, y].Neigbour.Add(grid[x + 1, y + 1]);
                    }
                    if (x < WidthNodes - 1 && y > 0)
                    {
                        grid[x, y].Neigbour.Add(grid[x + 1, y - 1]);
                    }
                    if (x > 0 && y < HeightNodes - 1)
                    {
                        grid[x, y].Neigbour.Add(grid[x - 1, y + 1]);
                    }
                }
            }
        }
    }

    class Node          //you will probably need to add another int for the A* bias
    {
        public Vector2 Location;
        public List<Node> Neigbour;
        public int TileType;

        public Node(int x, int y, int tileType)
        {
            this.Location = new Vector2(x, y);
            this.Neigbour = new List<Node>();
            this.TileType = tileType;
        }
    }
}

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
        public Rectangle RectanglePixel { get; set; }
        

        public Node[,] Grid { get; set; }
        public int WidthNodes {  get; set; }
        public int HeightNodes { get; set; }

        public List<Room> Rooms { get; set; }
        public List<Connection> Connections { get; set; }

        public Map(int height, int width, Texture2D grass, Texture2D wall)
        {
            this.grass = grass;
            this.wall = wall;

            
            this.WidthNodes = width / 32;
            this.HeightNodes = height / 32;

            this.Rectangle = new Rectangle(0, 0, WidthNodes, HeightNodes);
            this.RectanglePixel = new Rectangle(-16, -16, WidthNodes * 32, HeightNodes * 32);


            Grid = new Node[WidthNodes, HeightNodes];

            Connections = new List<Connection>();

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

        public void BuildMap()
        {
            this.Rooms = new List<Room>();
            int tries = 0;
            while (Rooms.Count < 25 && tries < 25)
            {
                Room newRoom = new Room(this);
                bool Overlap = false;

                foreach (Room r in Rooms)
                {
                    if (r.Size.Intersects(newRoom.Size))
                    {
                        Overlap = true; break;
                    }
                }
                if (Overlap == false && this.Rectangle.Contains(newRoom.Size))
                {
                    Rooms.Add(newRoom);
                    tries = 0;
                }
                else
                {
                    tries++;
                }
            }

            Connections = GenerateConnections();
        }

        public List<Connection> GenerateConnections()
        {
            

            for (int i = 0; i < Rooms.Count; i++)
            {
                for (int j = i + 1; j < Rooms.Count; j++)
                {
                    Room a = Rooms[i];
                    Room b = Rooms[j];

                    Connections.Add(new Connection(a, b));
                }
            }
            Connections = PRIMS();
            return Connections;
        }

        public List<Connection>  PRIMS()  
        {
            List<Connection> mst = new List<Connection>();
            List<Room> visited = new List<Room>();

            
            visited.Add(Rooms[0]);

            

            while(visited.Count < Rooms.Count)
            {
                Connection cheapest = null;
                foreach (Room r in visited)
                {
                    foreach (Connection c in Connections)
                    {
                        if(c.RoomA == r && !visited.Contains(c.RoomB) || 
                            c.RoomB == r && !visited.Contains(c.RoomA))
                        {
                            if (cheapest == null)
                            {
                                cheapest = c;
                            }
                            else if (c.Length < cheapest.Length)
                            {
                                cheapest = c;
                            }
                        }
                    }
                }
                mst.Add(cheapest);
                Connections.Remove(cheapest);
                if (visited.Contains(cheapest.RoomA))
                {
                    visited.Add(cheapest.RoomB);
                }
                else
                {
                    visited.Add(cheapest.RoomA);
                }
            }

            mst = AddCycles(mst);

            return mst;
        }

        private List<Connection> AddCycles(List<Connection> mst)
        {
            int extraCycles = 0;
            Random rand = new Random();
            List<Connection> toRemove = new List<Connection>();

            while (extraCycles < 4)
            {
                for (int i = 0; i < Connections.Count; i++)
                {
                    Connection c = Connections[i];
                    if (rand.Next(1, 50) == 1)
                    {
                        mst.Add(c);
                        Connections.Remove(c);
                        extraCycles++;
                        i++;

                        foreach (Connection c2 in Connections)
                        {
                            if (c2.RoomA == c.RoomA || c2.RoomA == c.RoomB || c2.RoomB == c.RoomA || c2.RoomB == c.RoomB)
                            {
                                toRemove.Add(c2);
                            }
                        }
                        foreach (Connection r in toRemove)
                        {
                            Connections.Remove(r);
                        }
                    }
                }
            }
            return mst;
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
        public void DrawRoomsHitbox(SpriteBatch spriteBatch, Texture2D pixel)
        {
            foreach (Room r in Rooms)
            {
                spriteBatch.Draw(pixel, new Rectangle((r.Size.X * 32 - 16) , (r.Size.Y * 32 - 16), (r.Size.Width * 32), 1), Color.Red);

                spriteBatch.Draw(pixel, new Rectangle((r.Size.X * 32 - 16), (r.Size.Y * 32 - 16) + (r.Size.Height * 32) - 1, (r.Size.Width * 32), 1), Color.Red);

                spriteBatch.Draw(pixel, new Rectangle((r.Size.X * 32 - 16), (r.Size.Y * 32 - 16), 1, (r.Size.Height * 32)), Color.Red);

                spriteBatch.Draw(pixel, new Rectangle((r.Size.X * 32 - 16) + (r.Size.Width * 32) - 1, (r.Size.Y * 32 - 16), 1, (r.Size.Height * 32)), Color.Red);
            }
            foreach (Connection c in Connections)
            {
                Vector2 edge = c.RoomB.Centre.ToVector2() - c.RoomA.Centre.ToVector2();

                float angle = MathF.Atan2(edge.Y, edge.X);
                float length = edge.Length() * 16;

                spriteBatch.Draw(
                    pixel,
                    c.RoomA.Centre.ToVector2() * 32,
                    null,
                    Color.Red,
                    angle,
                    new Vector2(0, 0.5f),
                    new Vector2(length, 1f),
                    SpriteEffects.None,
                    0
                );
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
            Walkable = true;

        }
    }

    public class Room
    {
        public Rectangle Size { get; set; }
        public Point Centre { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }


        public Room(Map Map)
        {
            Random rand = new Random();
            Width = rand.Next(12, 25);
            Height = rand.Next(12, 25);
            Size = new Rectangle(rand.Next(0, Map.WidthNodes) , rand.Next(0, Map.HeightNodes) , Width, Height); // make sure it always works with nodes not pixels
            Centre = Size.Center;
        }
    }

    public class Connection
    {
        public Room RoomA {  get; set; }
        public Room RoomB { get; set; }
        public double Length { get; set; }

        public Connection(Room a, Room b)
        {
            RoomA = a;
            RoomB = b;

            double distance = Math.Sqrt(Math.Pow(a.Centre.X - b.Centre.X, 2) + Math.Pow(a.Centre.Y - b.Centre.Y, 2));
            Length = distance;
        }
    }
}
    
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mono.Entities;
using System;
using System.Collections.Generic;


namespace mono
{
    public class Map
    {
        public Rectangle Rectangle { get; set; }
        public Rectangle RectanglePixel { get; set; }

        public Node[,] Grid { get; set; }

        public int WidthNodes { get; set; }
        public int HeightNodes { get; set; }

        public List<Room> Rooms { get; set; }
        public List<Connection> Connections { get; set; }


        Random rand = new Random();


        public Map(int height, int width)
        {
            this.WidthNodes = width / 32;
            this.HeightNodes = height / 32;

            this.Rectangle = new Rectangle(1, 1, WidthNodes - 1, HeightNodes - 1);
            this.RectanglePixel = new Rectangle(32, 32, WidthNodes * 32 - 32, HeightNodes * 32 - 32);


            this.Grid = new Node[WidthNodes, HeightNodes];

            this.Connections = new List<Connection>();

            for (int x = 0; x < WidthNodes; x++)
            {
                for (int y = 0; y < HeightNodes; y++)
                {
                    Node n = new Node(x, y);
                    Grid[x, y] = n;
                }
            }

            for (int x = 0; x < WidthNodes; x++)
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
                    if (x > 0 && y > 0)
                    {
                        Grid[x, y].Neigbour.Add(Grid[x - 1, y - 1]);

                    }
                    if (x < WidthNodes - 1)
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
            while (Rooms.Count < 50 && tries < 50)
            {
                Room newRoom = new Room(this);
                bool Overlap = false;

                foreach (Room r in Rooms)
                {
                    if (r.Rectangle.Intersects(newRoom.Rectangle))
                    {
                        Overlap = true; break;
                    }
                }
                if (Overlap == false && this.Rectangle.Contains(newRoom.Rectangle))
                {
                    Rooms.Add(newRoom);
                    tries = 0;
                }
                else
                {
                    tries++;
                }
            }

            this.Connections = GenerateConnections();

            foreach (Node n in Grid)
            {
                foreach (Room r in Rooms)
                {
                    if (r.RectanglePixel.Contains(n.Rectangle))
                    {
                        n.TileType = TileType.floor;
                        r.Nodes.Add(n);
                    }
                }
            }

            foreach (Connection c in Connections)   //moves a rectangle across connections to build corridors
            {
                Rectangle r = new Rectangle(c.RoomA.CentrePixel.X - 48, c.RoomA.CentrePixel.Y - 48, 112, 112);
                Vector2 vS = new Vector2(r.X, r.Y);
                Vector2 vT = new Vector2(c.RoomB.CentrePixel.X, c.RoomB.CentrePixel.Y);


                while (vS != vT)
                {
                    Vector2 distanceV = vT - vS;
                    double dist = Math.Sqrt(Math.Pow(distanceV.X, 2) + Math.Pow(distanceV.Y, 2));

                    double rectDist = dist / 64;

                    Vector2 transform = new Vector2((float)(distanceV.X / rectDist), (float)(distanceV.Y / rectDist));


                    if (dist < 64)
                    {
                        vS = vT;
                    }
                    else
                    {
                        vS += transform;
                    }

                    r.X = Convert.ToInt32(vS.X);
                    r.Y = Convert.ToInt32(vS.Y);

                    foreach (Node n in Grid)
                    {
                        if (n.Rectangle.Intersects(r))
                        {
                            n.TileType = TileType.floor;
                        }
                    }
                }


            }

            SetRoomTypes();

            foreach (Room r in Rooms)
            {
                r.DesignRoomLayout();
            }

            foreach (Node n in Grid)
            {
                n.SetTexture();
            }
        }
        private List<Connection> GenerateConnections()
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
            this.Connections = PRIMS(); //run prims algorithm 
            return Connections;
        }
        private List<Connection> PRIMS()
        {
            List<Connection> mst = new List<Connection>();
            List<Room> visited = new List<Room>();


            visited.Add(Rooms[0]);



            while (visited.Count < Rooms.Count)
            {
                Connection cheapest = null;
                foreach (Room r in visited)
                {
                    foreach (Connection c in Connections)
                    {
                        if (c.RoomA == r && !visited.Contains(c.RoomB) ||
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
                cheapest.RoomA.NumOfConnections += 1;
                cheapest.RoomB.NumOfConnections += 1;
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


            //adds cycles to the MST

            List<Connection> cycles = new List<Connection>();

            for (int i = 0; i < 4; i++)
            {
                int randomNum = rand.Next(0, Connections.Count - 1);
                Connection potentialC = Connections[randomNum];

                bool accept = true;
                foreach (Connection c in cycles) //check if there is a cycle being added which already connects to each of the potential connections rooms
                {
                    if (potentialC.RoomA == c.RoomA || potentialC.RoomA == c.RoomB || potentialC.RoomB == c.RoomA || potentialC.RoomB == c.RoomB)
                    {
                        i--;
                        accept = false;
                    }
                }
                if (accept == true)
                {
                    cycles.Add(potentialC);
                }
                potentialC.RoomA.NumOfConnections += 1;
                potentialC.RoomB.NumOfConnections += 1;
                Connections.Remove(potentialC);
            }
            foreach (Connection c in cycles) //adds chosen connections to the MST
            {
                mst.Add(c);
            }

            return mst;
        }
        private void SetRoomTypes()
        {
            SortedList<double, Room> RoomsByDist = new SortedList<double, Room>();
            double dist;

            for (int i = 1; i < Rooms.Count; i++)
            {
                dist = Math.Sqrt(Math.Pow(Rooms[i].Centre.X - Rooms[0].Centre.X, 2) + Math.Pow(Rooms[i].Centre.Y - Rooms[0].Centre.Y, 2));

                bool AddedToList = false;
                while (AddedToList == false) //checks to ensure 2 distances dont have the same key
                {
                    if (!RoomsByDist.ContainsKey(dist))
                    {
                        RoomsByDist.Add(dist, Rooms[i]);
                        AddedToList = true;
                    }
                    else
                    {
                        dist += 0.1;
                    }
                }

            }

            // selects rooms based on distance to spawn relative to other room distances
            //prioritizes rooms with a single connection 


            bool BossRoomSet = false;

            for (int j = 1; j < 3; j++)
            {
                for (int i = RoomsByDist.Count - 1; i > RoomsByDist.Count / 2; i--)
                {
                    if (RoomsByDist.GetValueAtIndex(i).NumOfConnections == j)
                    {
                        RoomsByDist.GetValueAtIndex(i).Type = RoomType.Boss;
                        BossRoomSet = true;
                        break;
                    }
                }
                if (BossRoomSet == true)
                {
                    break;
                }
            }

            bool ShopRoomSet = false;

            for (int j = 1; j < 3; j++)
            {
                for (int i = RoomsByDist.Count / 4 * 3; i > RoomsByDist.Count / 2; i--)   //starts from 3/4 out
                {
                    if (RoomsByDist.GetValueAtIndex(i).NumOfConnections == j && RoomsByDist.GetValueAtIndex(i).Type == RoomType.Empty)
                    {
                        RoomsByDist.GetValueAtIndex(i).Type = RoomType.Shop;
                        ShopRoomSet = true;
                        break;
                    }
                }
                if (ShopRoomSet == true)
                {
                    break;
                }
            }

            int LootRooms = 2;

            for (int j = 1; j < 3; j++)
            {
                for (int i = RoomsByDist.Count / 2; i > RoomsByDist.Count / 2; i--)   //starts from 1/2 out so loot room is closer
                {
                    if (RoomsByDist.GetValueAtIndex(i).NumOfConnections == j && RoomsByDist.GetValueAtIndex(i).Type == RoomType.Empty)
                    {
                        RoomsByDist.GetValueAtIndex(i).Type = RoomType.Shop;
                        LootRooms -= 1;
                        if (LootRooms == 0)
                        {
                            break;
                        }
                    }
                }
                if (LootRooms == 0)
                {
                    break;
                }
            }

            foreach (Room room in Rooms)
            {
                int random = rand.Next(0, 20);

                if (random != 0 && room.Type == RoomType.Empty)
                {
                    room.Type = RoomType.Enemy;
                }
            }

            Rooms[0].Type = RoomType.Empty;
        }


        public void DrawMap(SpriteBatch spriteBatch, Player p, int sHeight, int sWidth)
        {

            int sNHeight = sHeight / 32;
            int sNWidth = sWidth / 32;

            for (int i = Convert.ToInt32((p.Position.X / 32) - 2 - sNWidth / 2); i <= p.Position.X / 32 + 2 + sNWidth / 2; i++)
            {
                for (int j = Convert.ToInt32((p.Position.Y / 32) - 2 - sNHeight / 2); j <= p.Position.Y / 32 + 2 + sNHeight / 2; j++)
                {
                    if (i >= 0 && i < this.WidthNodes && j >= 0 && j < this.HeightNodes)
                    {
                        Node n = this.Grid[i, j];
                        spriteBatch.Draw(n.Texture, n.Position, null, Color.White, 0f, n.Centre, 1f, SpriteEffects.None, Layers.Background);
                    }
                }
            }
        }
        public void DrawRoomsHitbox(SpriteBatch spriteBatch)
        {
            foreach (Room r in Rooms)
            {
                spriteBatch.Draw(TM.GetTexture("pixel"), new Rectangle((r.RectanglePixel.X), (r.RectanglePixel.Y), (r.RectanglePixel.Width), 1), Color.Red);

                spriteBatch.Draw(TM.GetTexture("pixel"), new Rectangle((r.RectanglePixel.X), (r.RectanglePixel.Y) + (r.RectanglePixel.Height) - 1, (r.RectanglePixel.Width), 1), Color.Red);

                spriteBatch.Draw(TM.GetTexture("pixel"), new Rectangle((r.RectanglePixel.X), (r.RectanglePixel.Y), 1, (r.RectanglePixel.Height)), Color.Red);

                spriteBatch.Draw(TM.GetTexture("pixel"), new Rectangle((r.RectanglePixel.X) + (r.RectanglePixel.Width) - 1, (r.RectanglePixel.Y), 1, (r.RectanglePixel.Height)), Color.Red);
            }
            foreach (Connection c in Connections)
            {
                Vector2 edge = c.RoomB.Centre.ToVector2() - c.RoomA.Centre.ToVector2();

                float angle = MathF.Atan2(edge.Y, edge.X);
                float length = edge.Length() * 16;

                spriteBatch.Draw(
                    TM.GetTexture("pixel"),
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
        public Vector2 GridLocation { get; private set; }
        public Vector2 Position { get; private set; }
        public Rectangle Rectangle { get; set; }

        public Vector2 Size = new Vector2(32, 32);
        public Vector2 Centre = new Vector2(16, 16);

        public List<Node> Neigbour { get; set; }
        public TileType TileType { get; set; }

        public Texture2D Texture { get; private set; }

        public Node(int x, int y)
        {
            this.GridLocation = new Vector2(x, y);
            this.Position = new Vector2(x * 32, y * 32);
            this.Centre = new Vector2(16, 16);


            this.Rectangle = new Rectangle(Convert.ToInt32(Position.X - Size.X / 2), Convert.ToInt32(Position.Y - Size.Y / 2), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y));

            this.Neigbour = new List<Node>();
            Random rand = new Random();
        }

        public void SetTexture()
        {
            switch (TileType)
            {
                case TileType.wall:
                    this.Texture = TM.GetTexture("wall");
                    break;
                case TileType.water:
                    this.Texture = TM.GetTexture("water");
                    break;
                case TileType.floor:
                    this.Texture = TM.GetTexture("grass");
                    break;
            }

        }
    }

    public enum TileType
    {
        wall = 0,
        floor,
        water
    }

    public class Room
    {
        public Rectangle Rectangle { get; set; }
        public Rectangle RectanglePixel { get; set; }
        public Point Centre { get; set; }
        public Point CentrePixel { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public RoomType Type { get; set; }
        public int NumOfConnections { get; set; }

        public List<Node> Nodes { get; set; }
        Random rand;


        public Room(Map Map)
        {
            rand = new Random();
            Nodes = new List<Node>();

            this.Width = rand.Next(20, 35);
            this.Height = rand.Next(20, 35);
            int X = rand.Next(0, Map.WidthNodes);
            int Y = rand.Next(0, Map.HeightNodes);
            this.Rectangle = new Rectangle(X, Y, Width, Height);
            this.RectanglePixel = new Rectangle(X * 32 - 16, Y * 32 - 16, Width * 32, Height * 32);
            this.Centre = Rectangle.Center;
            this.CentrePixel = RectanglePixel.Center;
            this.NumOfConnections = 0;

        }

        public void DesignRoomLayout()
        {
            int randPuddle = rand.Next(0, 3);
            int randStartingPoint = rand.Next(0, Nodes.Count - 1);
            CreatePuddle(Nodes[randStartingPoint], 1);
        }

        public void CreatePuddle(Node n, int chance)   //expands water from random nore unsing decreased chance each expansion
        {
            int pud = rand.Next(1, chance);
            if (pud == 1 && n.TileType == TileType.floor)
            {
                n.TileType = TileType.water;
                foreach (Node neigbour in n.Neigbour)
                {
                    CreatePuddle(neigbour, chance + 1);
                }
            }
            
        }
    }


    public enum RoomType
    {
        Empty = 0,
        Enemy,
        Shop,
        Loot,
        Boss
    }

    public class Connection
    {
        public Room RoomA { get; set; }
        public Room RoomB { get; set; }
        public double Length { get; set; }

        public Connection(Room a, Room b)
        {
            this.RoomA = a;
            this.RoomB = b;

            double distance = Math.Sqrt(Math.Pow(a.Centre.X - b.Centre.X, 2) + Math.Pow(a.Centre.Y - b.Centre.Y, 2));
            this.Length = distance;
        }
    }
}
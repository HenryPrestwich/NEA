using mono.Main;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace mono
{
    internal static class AStar
    {
        public static Queue<Node> ASTAR(Vector2 enemy, Vector2 player, Map Map)
        {
            Node Start = Map.Grid[Convert.ToInt32(enemy.X) / 32, Convert.ToInt32(enemy.Y) / 32];
            Node Target = Map.Grid[Convert.ToInt32(player.X) / 32, Convert.ToInt32(player.Y) / 32];
          
            PriorityQueue<Node, double> Border = new PriorityQueue<Node, double>();
            Dictionary<Node, double> CostToNode = new Dictionary<Node, double>();
            Dictionary<Node, Node> CameFromNode = new Dictionary<Node, Node>();
            Border.Enqueue(Start, 0);
            CostToNode.Add(Start, 0); 


            if (Target.Walkable == false)
            {
                return null;
            }
            while (Border.Count > 0)
            {
                Node current = Border.Dequeue();

                if (current == Target)
                {
                    break;
                }

                foreach(Node n in current.Neigbour)
                {
                    if (n.Walkable == true)
                    {
                        double cost = CostToNode[current] + 1;
                        if (!CostToNode.ContainsKey(n) || cost < CostToNode[n])
                        {
                            CostToNode[n] = cost;
                            double priority = cost + Heuristic(n, Target);
                            Border.Enqueue(n, priority);
                            CameFromNode[n] = current;
                        }
                    }
                }
            }

            Node currentRev = Target;
            Stack<Node> pathRev = new Stack<Node>();
            Queue<Node> path = new Queue<Node>();
            while (currentRev != Start)
            {
                pathRev.Push(currentRev);
                currentRev = CameFromNode[currentRev];
            }
            while (pathRev.Count > 0)
            {
                path.Enqueue(pathRev.Pop());
            }
             
            return path;
        }

        public static double Heuristic(Node Start, Node Target)
        {
            int a = Convert.ToInt32(Target.GridLocation.X - Start.GridLocation.X);
            int b = Convert.ToInt32(Target.GridLocation.Y - Start.GridLocation.Y);
            double C = Math.Pow(a, 2) + Math.Pow(b, 2);
            double D = Math.Sqrt(C);
            return D;
        }
    }
}

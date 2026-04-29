using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace mono
{
    internal static class AStar
    {
        public static List<Node> ASTAR(Node Start, Node Target)
        {
            PriorityQueue<Node, double> Border = new PriorityQueue<Node, double>();
            Dictionary<Node, double> CostToNode = new Dictionary<Node, double>();
            Dictionary<Node, Node> CameFromNode = new Dictionary<Node, Node>();
            Border.Enqueue(Start, 0);
            CostToNode.Add(Start, 0);

            while (Border.Count > 0)
            {
                Node current = Border.Dequeue();

                if (current == Target)
                {
                    break;
                }

                foreach(Node n in current.Neigbour)
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

            return new List<Node>();
        }

        public static double Heuristic(Node Start, Node Target)
        {
            int a = Convert.ToInt32(Target.Location.X - Start.Location.X);
            int b = Convert.ToInt32(Target.Location.Y - Start.Location.Y);
            double C = Math.Pow(a, 2) + Math.Pow(b, 2);
            double D = Math.Sqrt(C);
            return D;
        }
    }
}

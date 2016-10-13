using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example
{
    
    //public class Edge
    //{
    //    public readonly Node From;
    //    public readonly Node To;

    //    public Edge(Node first, Node second)
    //    {
    //        this.From = first;
    //        this.To = second;
    //    }

    //    public Node OtherNode(Node node)
    //    {
    //        if (From == node) return To;
    //        return From;
    //    }

    //    public static double Distance(Node one, Node two)
    //    {
    //        var a = Math.Abs(one.Point.X - two.Point.X);
    //        var b = Math.Abs(one.Point.Y - two.Point.Y);
    //        return Math.Sqrt(a * a + b * b);
    //    }
    //}

    //public class Node
    //{
    //    readonly List<Edge> edges = new List<Edge>();
    //    public readonly Point Point;
    //    public string Tag;

    //    public Node(Point point, string tag)
    //    {
    //        Point = point;
    //        Tag = tag;
    //    }

    //    public IEnumerable<Node> IncidentNodes
    //    {
    //        get
    //        {
    //            return edges.Select(z => z.OtherNode(this));
    //        }
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        var n = obj as Node;
    //        return n.Point.X == this.Point.X && n.Point.Y == this.Point.Y;
    //    }

    //    public override int GetHashCode()
    //    {
    //        return this.Point.GetHashCode();
    //    }

    //    public static Edge Connect(Node node1, Node node2)
    //    {
    //        var edge = new Edge(node1, node2);
    //        node1.edges.Add(edge);
    //        node2.edges.Add(edge);
    //        return edge;
    //    }
    //}

    //public class Graph
    //{
    //    public List<Node> nodes = new List<Node>();

    //    public void AddNode(Point point, string tag)
    //    {
    //        nodes.Add(new Node(point, tag));
    //    }

    //    public IEnumerable<Node> Nodes
    //    {
    //        get
    //        {
    //            foreach (var node in nodes) yield return node;
    //        }
    //    }

    //    public void PossibleConnect(List<Sector> walls)
    //    {
    //        var fl = true;
    //        foreach (var node in nodes)
    //            foreach (var other in nodes.Where(x => !node.IncidentNodes.Contains(x) && x != node))
    //            {
    //                foreach (var wall in walls)
    //                    if (wall.SecIntersec(new Sector(new Point(node.Point.X, node.Point.Y), new Point(other.Point.X, other.Point.Y))))
    //                    {
    //                        fl = false;
    //                        break;
    //                    }
    //                if (fl)
    //                    Node.Connect(node, other);
    //                fl = true;
    //            }
    //    }
    //    public int GetNodeIndex(Point point)
    //    {
    //        var result = -1;
    //        for (var i = 0; i < this.nodes.Count(); i++)
    //            if (this.nodes[i].Point.Equals(point))
    //                result = i;
    //        return result;
    //    }
    //}
}

using System;
using AIRLab.Mathematics;
using ClientBase;
using CommonTypes;
using CVARC.Basic.Controllers;
using CVARC.Network;
using RepairTheStarship.Sensors;
using System.Collections.Generic;
using System.Linq;
using CVARC.Basic.Sensors;
using System.Timers;
using System.Diagnostics;
using System.Threading;
using Microsoft.SqlServer.Server;
using RepairTheStarship;
using SlimDX.X3DAudio;

namespace Example
{
    enum Colors { Green = 1, Blue = 2, Red1 = -2, Green1 = -1, Red = 3, Blue1 = -3 };

    public class Edge
    {
        public readonly Node From;
        public readonly Node To;

        public Edge(Node first, Node second)
        {
            From = first;
            To = second;
        }

        public Node OtherNode(Node node)
        {
            return From == node ? To : From;
        }
    }

    public class Geometry
    {
        public static double Distance(Point one, Point two)
        {
            var a = Math.Abs(one.X - two.X);
            var b = Math.Abs(one.Y - two.Y);
            return Math.Sqrt(a * a + b * b);
        }

        public static double GetAngle(Point one, Point two, double angle)
        {
            angle = angle % 360;
            angle = angle * Math.PI / 180;
            var vectorOfTargets = two - one;
            var currentVector = new Point(Math.Cos(angle), Math.Sin(angle));
            var vectorProduct = currentVector.X * vectorOfTargets.Y - currentVector.Y * vectorOfTargets.X;
            var scalarProduct = currentVector.X * vectorOfTargets.X + currentVector.Y * vectorOfTargets.Y;
            var newAngle = Math.Acos(scalarProduct / (Distance(new Point(0, 0), currentVector) * Distance(new Point(0, 0), vectorOfTargets)));
            vectorProduct = vectorProduct > 0 ? 1 : -1;
            return newAngle * vectorProduct;
        }
    }

    public class Node
    {
        readonly List<Edge> edges = new List<Edge>();
        public readonly Point Point;
        public readonly Point OriginalPoint;
        public bool Visited = false;
        public bool Explored = false;
        public string Tag { get; set; }

        public Node(Point point, string tag, bool visited)
        {
            Visited = visited;
            Point = point;
            Tag = tag;
        }

        public Node(Point point, Point originalPoint, string tag)
        {
            Point = point;
            Tag = tag;
            OriginalPoint = originalPoint;
        }

        public IEnumerable<Node> IncidentNodes
        {
            get { return edges.Select(z => z.OtherNode(this)); }
        }

        public override bool Equals(object obj)
        {
            var n = obj as Node;
            return n.Point.X == this.Point.X && n.Point.Y == Point.Y;
        }

        public override int GetHashCode()
        {
            return Point.GetHashCode();
        }

        public static Edge Connect(Node node1, Node node2)
        {
            var edge = new Edge(node1, node2);
            node1.edges.Add(edge);
            node2.edges.Add(edge);
            return edge;
        }

        public static void Disconnect(Node node1, Node node2)
        {
            node1.edges.Remove(node1.edges.First(w => w.OtherNode(node1) == node2));
            node2.edges.Remove(node2.edges.First(w => w.OtherNode(node2) == node1));
        }

        public override string ToString()
        {
            return String.Format("{0}, X: {1}, Y: {2}", Tag, Point.X, Point.Y);
        }
    }

    public class Graph
    {
        private List<Node> nodes = new List<Node>();

        public void AddNode(Point point, string tag, bool visited=false)
        {
            nodes.Add(new Node(point, tag, visited));
        }

        public void AddNode(Point point, Point originalPoint, string tag)
        {
            nodes.Add(new Node(point, originalPoint, tag));
        }

        public IEnumerable<Node> Nodes
        {
            get { return nodes; }
        }

        public Node GetNearestToPoint(Point point, Func<string, bool> predicate)
        {
            var minDist = double.MaxValue;
            Node result = null;
            foreach(var e in nodes)
            {
                var distance = Geometry.Distance(e.Point, point);
                if (!(distance < minDist) || !predicate(e.Tag)) continue;
                result = e;
                minDist = distance;
            }
            return result;
        }

        public Node GetNearestToPoint(Point point)
        {
            return GetNearestToPoint(point, x => true);
        }

        public void PossibleConnect(List<Sector> walls)
        {
            var isConnectPossible = true;
            foreach (var node in nodes.Where(x => x.Explored))
                foreach (var other in nodes.Where(x => x != node && Geometry.Distance(x.Point, node.Point) < 70 && x.Explored))
                {
                    var sector = new Sector(new Point(node.Point.X, node.Point.Y),
                        new Point(other.Point.X, other.Point.Y));
                    if (walls.Any(wall => wall.SecIntersec(sector)))
                        isConnectPossible = false;
                    if (isConnectPossible)
                        Node.Connect(node, other);
                    //никогда не сработает из-за условия цикла
                    else if (node.IncidentNodes.Contains(other))
                        Node.Disconnect(node, other);
                    isConnectPossible = true;
                }
        }

        public Node GetNodeByPoint(Point point)
        {
            return nodes.FirstOrDefault(e => e.Point.Equals(point));
        }
    }

    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static bool operator <=(Point a, Point b)
        {
            return (a.X < b.X) || (a.Y < b.Y) || (a.X == b.X && a.Y == b.Y);
        }

        public static bool operator >=(Point a, Point b)
        {
            return (a.X > b.X) || (a.Y > b.Y) || (a.X == b.X && a.Y == b.Y);
        }

        public override bool Equals(object obj)
        {
            var p = obj as Point;
            return Math.Abs(X - p.X) < 0.001 && Math.Abs(Y - p.Y) < 0.001;
        }

        public static Point operator -(Point objTwo, object objOne)
        {
            var pointOne = objOne as Point;
            var pointTwo = objTwo as Point;
            return new Point(pointTwo.X - pointOne.X, pointTwo.Y - pointOne.Y);
        }
    }

    public class Sector
    {
        public Point Start;
        public Point End;
        public Sector(Point start, Point end)
        {
            Start = start;
            End = end;
        }

        public bool InSec(Point point)
        {
            var inRect = ((point <= Start) && (point >= End)) || ((point <= End) && (point >= Start));
            return inRect && ((point.X - Start.X) / (End.X - Start.X) == (point.Y - Start.Y) / (End.Y - Start.Y));
        }

        public bool SecIntersec(Sector other)
        {
            var otherStart = other.Start;
            var otherEnd = other.End;
            var endsIntersec = InSec(otherStart) || InSec(otherEnd) || other.InSec(Start) || other.InSec(End);
            double lenght = (End.X - Start.X) * (otherEnd.Y - otherStart.Y) - (End.Y - Start.Y) * (otherEnd.X - otherStart.X);
            double rH = (Start.Y - otherStart.Y) * (otherEnd.X - otherStart.X) - (Start.X - otherStart.X) * (otherEnd.Y - otherStart.Y);
            double sH = (Start.Y - otherStart.Y) * (End.X - Start.X) - (Start.X - otherStart.X) * (End.Y - Start.Y);
            double normFirst = rH / lenght;
            double normSecond = sH / lenght;
            return (normFirst >= 0 && normFirst <= 1 && normSecond >= 0 && normSecond <= 1) || endsIntersec;
        }
    }

    class DijkstraData
    {
        public Node Previous { get; set; }
        public double PathLength { get; set; }
    }

    internal class Program
    {
        private static readonly ClientSettings Settings = new ClientSettings
        {
            Side = Side.Right,
            LevelName = LevelName.Level3,
            MapNumber = 2,
            BotName = Bots.Azura
        };
        private static PositionSensorsData sensorsData = null;
        private static Server<PositionSensorsData> server = null;
        private static Graph mapInfo = null;
        private static string hasDetail = "";
        private static Node currentNode = null;
        private static bool isOver = false;
        private static bool isMoving = false;
        private static bool changeRoute = false;
        private static int myNumber = 0;
        private static int opponentNumber = 1;
        private static List<Sector> walls = new List<Sector>();

        private static void Main(string[] args)
        {
            server = new CvarcClient(args, Settings).GetServer<PositionSensorsData>();
            Console.Write(server);
            var helloPackageAns = server.Run();
            if (helloPackageAns.RealSide.ToString() == "Right")
            {
                myNumber = 1;
                opponentNumber = 0;
            }
            sensorsData = server.SendCommand(new Command { AngularVelocity = Angle.FromGrad(0), Time = 0.001 });
            mapInfo = new Graph();
            GetMapInfo(sensorsData.MapSensor.MapItems, helloPackageAns.RealSide.ToString());
            var myTimer = new System.Timers.Timer {Interval = 1};
            myTimer.Elapsed += FixStarship;
            myTimer.Start();
            while (!isOver) Thread.Sleep(2000);
            server.Exit();
        }

        static void FixStarship(object source, ElapsedEventArgs e)
        {
            if (isMoving) return;
            isMoving = true;
            var path = GetBestPath(false, false);
            if (path.Count == 0)
            {
                if (mapInfo.Nodes.Count(x => x.Tag.Contains("Detail")) != 0)
                    path = GetBestPath(true, false);
                if (path.Count == 0 && !mapInfo.Nodes.Any(x => x.Tag.Contains("Detail")) &&
                    !mapInfo.Nodes.Any(x => x.Tag.Contains("Socket")))
                {
                    isOver = true;
                    return;
                }
                if (mapInfo.Nodes.Count(x => !x.Visited) != 0)
                    path = GetBestPath(true, true);
                else
                {
                    sensorsData = server.SendCommand(new Command {AngularVelocity = Angle.FromGrad(0), Time = 0.5});
                    isMoving = false;
                    return;
                }
            }
            if (path.Count > 1)
                for (var i = 1; i < path.Count; i++)
                    if (!changeRoute)
                    {
                        if (Geometry.Distance(path[path.Count - 1].Point, currentNode.Point) < 70 && path[path.Count - 1].Tag != "Waypoint")
                        {
                            if (!sensorsData.MapSensor.MapItems.Select(x => x.Tag)
                                .Contains(path[path.Count - 1].Tag.Split(' ')[0]))
                            {
                                path[path.Count - 1].Tag = "Waypoint";
                                break;
                            }
                            if (
                                sensorsData.MapSensor.MapItems.First(x => x.Tag == path[path.Count - 1].Tag.Split(' ')[0])
                                    .Tag.Contains("Detail"))
                            {
                                var item =
                                    sensorsData.MapSensor.MapItems.First(
                                        x => x.Tag == path[path.Count - 1].Tag.Split(' ')[0]);
                                path[path.Count - 1].Point.X = item.X;
                                path[path.Count - 1].Point.Y = item.Y;
                            }
                        }
                        Move(path[i]);
                        UpdateMap(sensorsData.MapSensor.MapItems);
                    }
                    else break;
            else if (!changeRoute) Move(path[0]);
            changeRoute = false;
            isMoving = false;
        }

        static void Turn(double angle, double origAngle)
        {
            if (angle >= 0) sensorsData = server.SendCommand(new Command { AngularVelocity = Angle.FromGrad(90), Time = angle / 90 });
            else sensorsData = server.SendCommand(new Command { AngularVelocity = Angle.FromGrad(-90), Time = -angle / 90 });
            while (Math.Abs(angle + origAngle - sensorsData.Position.PositionsData[myNumber].Angle % 360) % 360 > 5)
            {
                if (360 - Math.Abs(angle + origAngle - sensorsData.Position.PositionsData[myNumber].Angle % 360) % 360 < 5) break;
                angle = (angle + origAngle - sensorsData.Position.PositionsData[myNumber].Angle % 360) % 360;
                origAngle = sensorsData.Position.PositionsData[myNumber].Angle % 360;
                sensorsData = server.SendCommand(new Command { LinearVelocity = -50, Time = 0.1 });
                if (angle >= 0) sensorsData = server.SendCommand(new Command { AngularVelocity = Angle.FromGrad(90), Time = angle / 90 });
                else sensorsData = server.SendCommand(new Command { AngularVelocity = Angle.FromGrad(-90), Time = -angle / 90 });
            }
        }

        static void Move(Node to)
        {
            var position = new Point(sensorsData.Position.PositionsData[myNumber].X, sensorsData.Position.PositionsData[myNumber].Y);
            var origAngle = sensorsData.Position.PositionsData[myNumber].Angle % 360;
            var angle = (Geometry.GetAngle(position, to.Point, origAngle) % (2 * Math.PI)) * 180 / Math.PI;
            var velocity = 1;
            if (Math.Abs(angle) > 100 && to.Tag == "Waypoint")
            {
                velocity = -1;
                angle -= angle > 0 ? 180 : -180;
            }
            Turn(angle, origAngle);
            var dist = Geometry.Distance(position, to.Point);
            if (to.Tag.Contains("Detail") && dist > 13) dist -= 13;
            if (sensorsData.DetailsInfo.HasGrippedDetail && dist > 5 && velocity == 1) dist -= 5;
            for (var i = 0; i < 2; i++)
            {
                position = new Point(sensorsData.Position.PositionsData[myNumber].X, sensorsData.Position.PositionsData[myNumber].Y);
                var opponentPos = new Point(sensorsData.Position.PositionsData[opponentNumber].X, sensorsData.Position.PositionsData[opponentNumber].Y);
                if (to.IncidentNodes.Contains(mapInfo.GetNearestToPoint(opponentPos)) &&
                    Geometry.Distance(position, opponentPos) < 75 && Geometry.Distance(to.Point, opponentPos) < 75)
                {
                    sensorsData = server.SendCommand(new Command { AngularVelocity = Angle.FromGrad(0), Time = 0.3 });
                    changeRoute = true;
                    return;
                }
                sensorsData = server.SendCommand(new Command { LinearVelocity = 50 * velocity, Time = dist / 100 });
            }
            if (to.Tag.Contains("Socket") && to.Tag.Contains(hasDetail))
            {
                sensorsData = sensorsData = server.SendCommand(new Command { Action = CommandAction.Release, Time = 1 });
                to.Tag = "Waypoint";
                mapInfo.GetNearestToPoint(to.Point, x => x.Contains("Up") || x.Contains("Left") || x.Contains("Right") || x.Contains("Down")).Tag = "Waypoint";
                hasDetail = "";
                currentNode = to;
                return;
            }
            if (to.Tag.Contains("Detail") && !sensorsData.DetailsInfo.HasGrippedDetail)
            {
                hasDetail = to.Tag.Split('D')[0];
                sensorsData = sensorsData = server.SendCommand(new Command { Action = CommandAction.Grip, Time = 1 });
                if (!sensorsData.DetailsInfo.HasGrippedDetail)
                    Move(to);
                to.Tag = "Waypoint";
            }
            currentNode = to;
        }

        static List<Node> GetBestPath(bool anyPath, bool explore)
        {
            var paths = new List<List<Node>>();
            if (!explore)
                foreach (var node in mapInfo.Nodes)
                {
                    if (node.Tag.Contains("Detail") && hasDetail == "")
                        paths.Add(Dijkstra(mapInfo, currentNode, node, anyPath));
                    if (node.Tag.Contains("Socket") && node.Tag.Contains(hasDetail) && hasDetail != "")
                        paths.Add(Dijkstra(mapInfo, currentNode, node, anyPath));
                }
            else
                paths.AddRange(from node in mapInfo.Nodes where !node.Visited && Geometry.Distance(currentNode.Point, node.Point) < 120 && node.Tag == "Waypoint"
                               select Dijkstra(mapInfo, currentNode, node, anyPath));
            var bestPath = new List<Node>();
            var currentMinLen = double.MaxValue;
            for (var z = 0; z < 2; z++)
            {
                if (z > 0 && bestPath.Count != 0) break;
                foreach (var path in paths.Where(x => x != null && x[x.Count - 1].Tag != (z == 1? "Waypoint" : "")))
                {
                    var distance = 0.0;
                    for (var i = 0; i < path.Count - 1; i++)
                        distance += Geometry.Distance(path[i].Point, path[i + 1].Point);
                    if (distance < currentMinLen)
                    {
                        bestPath = path;
                        currentMinLen = distance;
                    }
                }
            }
            return bestPath;
        }

        public static List<Node> Dijkstra(Graph graph, Node start, Node end, bool anyPath)
        {
            var opponentPos = new Point(sensorsData.Position.PositionsData[opponentNumber].X, sensorsData.Position.PositionsData[opponentNumber].Y);
            var notVisited = graph.Nodes.ToList();
            var track = new Dictionary<Node, DijkstraData>();
            track[start] = new DijkstraData { PathLength = 0, Previous = null };
            while (true)
            {
                Node toOpen = null;
                var bestPrice = double.PositiveInfinity;
                foreach (var e in notVisited.Where(e => track.ContainsKey(e) && track[e].PathLength < bestPrice))
                {
                    bestPrice = track[e].PathLength;
                    toOpen = e;
                }
                if (toOpen == null) return null;
                if (toOpen == end) break;
                foreach (var e in toOpen.IncidentNodes.Where(x =>
                         (Geometry.Distance(x.Point, opponentPos) > 50 || !x.IncidentNodes.Contains(mapInfo.GetNearestToPoint(opponentPos)) || anyPath)
                         && Geometry.Distance(x.Point, toOpen.Point) < 70))
                {
                    var currentPrice = track[toOpen].PathLength + Geometry.Distance(toOpen.Point, e.Point);
                    var nextNode = e;
                    if (!track.ContainsKey(nextNode) || track[nextNode].PathLength > currentPrice)
                        track[nextNode] = new DijkstraData { Previous = toOpen, PathLength = currentPrice };
                }
                notVisited.Remove(toOpen);
            }
            var result = new List<Node>();
            while (end != null)
            {
                result.Add(end);
                end = track[end].Previous;
            }
            result.Reverse();
            return result;
        }

        public static void UpdateMap(MapItem[] items)
        {
            var b = new Stopwatch();
            b.Start();
            var delta1 = 34;
            var delta2 = 12;
            var delta3 = 7;
            foreach (var e in items)
            {
                if (e.Tag.Contains("Detail") && !mapInfo.Nodes.Contains(new Node(new Point(e.X, e.Y), new Point(e.X, e.Y), e.Tag)))
                {
                    var asrt = mapInfo.GetNodeByPoint(new Point(e.X, e.Y));
                    if (asrt != null) asrt.Tag = e.Tag;
                    else mapInfo.AddNode(new Point(e.X, e.Y), new Point(e.X, e.Y), e.Tag);
                    mapInfo.AddNode(new Point(-e.X, e.Y), new Point(e.X, e.Y), NegativeColor(e.Tag));
                }
                else if (e.Tag.Contains("Horizontal"))
                {
                    if (!walls.Contains(new Sector(new Point(e.X - delta1, e.Y + delta3),
                        new Point(e.X + delta1, e.Y + delta3))))
                    {
                        walls.Add(new Sector(new Point(e.X - delta1, e.Y), new Point(e.X + delta1, e.Y)));
                        walls.Add(new Sector(new Point(-e.X - delta1, e.Y), new Point(-e.X + delta1, e.Y)));
                    }
                    if (!e.Tag.Contains("Socket")) continue;
                    if (!mapInfo.Nodes.Contains(new Node(new Point(e.X, e.Y + delta2), new Point(e.X, e.Y),
                        e.Tag + " Up")))
                    {
                        mapInfo.AddNode(new Point(e.X, e.Y + delta2), new Point(e.X, e.Y), e.Tag + " Up");
                        mapInfo.AddNode(new Point(e.X, e.Y - delta2), new Point(e.X, e.Y), e.Tag + " Down");
                        mapInfo.AddNode(new Point(-e.X, e.Y + delta2), new Point(e.X, e.Y), NegativeColor(e.Tag) + " Up");
                        mapInfo.AddNode(new Point(-e.X, e.Y - delta2), new Point(e.X, e.Y), NegativeColor(e.Tag) + " Down");
                    }
                }
                else if (e.Tag.Contains("Vertical"))
                {
                    if (!walls.Contains(new Sector(new Point(e.X - delta3, e.Y - delta1),
                        new Point(e.X - delta3, e.Y + delta1))))
                    {
                        walls.Add(new Sector(new Point(e.X, e.Y - delta1), new Point(e.X, e.Y + delta1)));
                        walls.Add(new Sector(new Point(-e.X, e.Y - delta1), new Point(-e.X, e.Y + delta1)));
                    }
                    if (!e.Tag.Contains("Socket")) continue;
                    if (!mapInfo.Nodes.Contains(new Node(new Point(e.X + delta2, e.Y), new Point(e.X, e.Y), e.Tag + " Right")))
                    {
                        mapInfo.AddNode(new Point(e.X + delta2, e.Y), e.Tag + " Right", true);
                        mapInfo.AddNode(new Point(e.X - delta2, e.Y), e.Tag + " Left", true);
                        mapInfo.AddNode(new Point(-e.X + delta2, e.Y), NegativeColor(e.Tag) + " Right", true);
                        mapInfo.AddNode(new Point(-e.X - delta2, e.Y), NegativeColor(e.Tag) + " Left", true);
                    }
                }
            }
            mapInfo.GetNodeByPoint(
                new Point(Math.Round(-(currentNode.Point.X + (currentNode.Point.X % 1 == 0 ? 0 : 0.2)), 1),
                    currentNode.Point.Y)).Visited = true;
            foreach (var node in mapInfo.Nodes.Where(node => Geometry.Distance(node.Point, currentNode.Point) < 80))
            {
                node.Explored = true;
                var a = new Point(Math.Round(-(node.Point.X + (node.Point.X % 1 == 0 ? 0 : 0.2)), 1), node.Point.Y);
                mapInfo.GetNodeByPoint(a).Explored = true;
            }
            Console.WriteLine("Update before connect " + b.ElapsedMilliseconds);
            if (!currentNode.Visited) mapInfo.PossibleConnect(walls);
            currentNode.Visited = true;
            b.Stop();
            Console.WriteLine("Update after connect "+b.ElapsedMilliseconds);
        }

        public static string NegativeColor(string str)
        {
            for (var i = 1; i < 4; i++)
                if (str.Contains(((Colors) i).ToString()))
                    return str.Replace(((Colors) i).ToString(), ((Colors) (i*-1)).ToString()).Replace("1", "");
            return null;
        }

        public static void GetMapInfo(MapItem[] items, string realSide)
        {
            for (var x = -125.1; x < 150; x += 50)
                for (var y = -75; y < 100; y += 50)
                    mapInfo.AddNode(new Point(x, y), "Waypoint");
            mapInfo.AddNode(new Point(-135, 85), "RedRobot");
            mapInfo.AddNode(new Point(135, 85), "BlueRobot");
            currentNode = mapInfo.GetNodeByPoint(new Point(realSide == "Right" ? 135 : -135, 85));
            UpdateMap(items);
        }
    }
}
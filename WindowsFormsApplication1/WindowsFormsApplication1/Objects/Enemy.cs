using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace WindowsFormsApplication1.Objects
{
    public class Enemy : IMovable
    {
        public Point Position { get; set; }
        public string Image
        {
            get
            {
                if (IsDead) return "DeadEnemy.png";
                if (Animated > 0)
                {
                    Animated--;
                    return Animated > 7 ? Animated > 15 ? "EnemySweep1.png" : "EnemySweep2.png" : "EnemySweep3.png";
                }
                if (Animated < 0 && (Animated < -20 || Animated > -10))
                {
                    Animated++;
                    return Animated < -20 ? "EnemyWalk1.png" : "EnemyWalk2.png";
                }
                if (Animated < 0) Animated++;
                return "Enemy.png";
            }
        }
        public Game Game { get; }
        public int DrawingPriority => IsDead ? 5 : 3;
        public double Angle { get; private set; }
        public bool IsDead;
        public Stack<Point> CurrentPath;
        private Point point;
        private readonly Stopwatch lastShot;
        public int Animated { get; private set; }

        public Enemy(int x, int y, Game game)
        {
            lastShot = new Stopwatch();
            lastShot.Start();
            CurrentPath = new Stack<Point>();
            Angle = 0;
            IsDead = false;
            Game = game;
            Position = new Point(x, y);
        }

        public void Kill()
        {
            IsDead = true;
            Position = new Point((int)(Position.X+20*Math.Cos(Angle)), (int)(Position.Y - 20*Math.Sin(Angle)));
        }

        public void Move()
        {
            const int speed = 5;
            if (Geometry.Distance(Game.Hero.Position, Position) > 400) return;
            var heroPos = Game.Hero.Position;
            var target = CurrentPath != null && CurrentPath.Count != 0 ? CurrentPath.ElementAt(CurrentPath.Count - 1) : new Point(0, 0);
            if (CurrentPath == null || CurrentPath.Count == 0 || (Math.Abs(target.X - heroPos.X) > Game.ObjectSize && Math.Abs(target.Y - heroPos.Y) > Game.ObjectSize))
            {
                CurrentPath = AStar(Game.Graph, new MyPoint(Position.X / Game.ObjectSize, Position.Y / Game.ObjectSize), new MyPoint(heroPos.X / Game.ObjectSize, heroPos.Y / Game.ObjectSize));
                if (CurrentPath == null) return;
                point = CurrentPath.Pop();
            }

            if (Position.X > point.X)
            {
                Position = Position.X - point.X > speed ? new Point(Position.X - speed, Position.Y) : new Point(point.X, Position.Y);
                Angle = -Math.PI;
            }
            if (Position.X < point.X)
            {
                Position = point.X - Position.X > speed ? new Point(Position.X + speed, Position.Y) : new Point(point.X, Position.Y);
                Angle = 0;
            }
            if (Position.Y > point.Y)
            {
                Position = Position.Y - point.Y > speed ? new Point(Position.X, Position.Y - speed) : new Point(Position.X, point.Y);
                Angle = -Math.PI/2;
            }
            if (Position.Y < point.Y)
            {
                Position = Position.Y - point.Y > speed ? new Point(Position.X, Position.Y + speed) : new Point(Position.X, point.Y);
                Angle = Math.PI/2;
            }
            if (Animated == 0) Animated = -30;
            if (Geometry.Distance(Position, Game.Hero.Position) < 60)
            {
                Angle = Geometry.GetAngle(Position, Game.Hero.Position, Angle);
                Shoot();
            }
            if (CurrentPath.Count != 0 && Position.X == point.X && Position.Y == point.Y) 
                point = CurrentPath.Pop();
        }

        private void Shoot()
        {
            lastShot.Stop();
            if (lastShot.ElapsedMilliseconds < 1000)
            {
                lastShot.Start();
                return;
            }
            lastShot.Restart();
            Animated = 22;
        }

        public Stack<Point> AStar(char[,] matrix, MyPoint start, MyPoint end)
        {
            var visited = new HashSet<MyPoint>();
            var open = new HashSet<MyPoint> { start };
            var gScore = new Dictionary<MyPoint, double>();
            var fScore = new Dictionary<MyPoint, double>();
            var cameFrom = new Dictionary<MyPoint, MyPoint>();
            gScore.Add(start, 0);
            fScore.Add(start, Geometry.Distance(start, end));
            while (open.Count > 0)
            {
                double[] minFScore = {double.MaxValue};
                var current = new MyPoint(0, 0);
                foreach (var e in open.Where(e => fScore[e] < minFScore[0]))
                {
                    minFScore[0] = fScore[e];
                    current = e;
                }
                if (current.Equals(end)) 
                    return GetPath(cameFrom, end);
                open.Remove(current);
                visited.Add(current);
                for (var i = -1; i <= 1; i++)
                    for (var j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue;
                        var neighbor = new MyPoint(current.X + i, current.Y + j);
                        if (visited.Contains(neighbor) || matrix[neighbor.X, neighbor.Y] == 'w' || matrix[neighbor.X, neighbor.Y] == 'e')
                            continue;
                        var tentativeGScore = gScore[current] + Math.Sqrt(i * i + j * j);
                        if (open.Contains(neighbor) && !(tentativeGScore < gScore[neighbor])) continue;
                        if (!cameFrom.ContainsKey(neighbor)) cameFrom.Add(neighbor, current);
                        else cameFrom[neighbor] = current;
                        if (!gScore.ContainsKey(neighbor)) gScore.Add(neighbor, tentativeGScore);
                        else gScore[neighbor] = tentativeGScore;
                        if (!fScore.ContainsKey(neighbor)) fScore.Add(neighbor, gScore[neighbor] + Geometry.Distance(neighbor, end));
                        else fScore[neighbor] = gScore[neighbor] + Geometry.Distance(neighbor, end);
                        if (!open.Contains(neighbor)) open.Add(neighbor);
                    }
            }
            return null;
        }

        public Stack<Point> GetPath(Dictionary<MyPoint, MyPoint> cameFrom, MyPoint current)
        {
            var path = new Stack<Point>();
            path.Push(new Point(current.X*Game.ObjectSize, current.Y*Game.ObjectSize));
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Push(new Point(current.X * Game.ObjectSize, current.Y * Game.ObjectSize));
            }
            return path;
        }
    }
}
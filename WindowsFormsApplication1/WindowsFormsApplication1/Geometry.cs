using System;
using System.Drawing;

namespace WindowsFormsApplication1
{
    public class Geometry
    {
        public static double Distance(double x1, double y1, double x2, double y2)
        {
            var a = Math.Abs(x1 - x2);
            var b = Math.Abs(y1 - y2);
            return Math.Sqrt(a * a + b * b);
        }

        public static double Distance(Point p1, Point p2)
        {
            var a = Math.Abs(p1.X - p2.X);
            var b = Math.Abs(p1.Y - p2.Y);
            return Math.Sqrt(a * a + b * b);
        }

        public static double Distance(MyPoint p1, MyPoint p2)
        {
            var a = Math.Abs(p1.X - p2.X);
            var b = Math.Abs(p1.Y - p2.Y);
            return Math.Sqrt(a * a + b * b);
        }

        public static double GetAngle(Point one, Point two, double angle)
        {
            angle = angle % 360;
            angle = angle * Math.PI / 180;
            var vectorOfTargets = new Point(two.X - one.X, two.Y - one.Y);
            var currentVectorX = Math.Cos(angle);
            var currentVectorY = Math.Sin(angle);
            var vectorProduct = currentVectorX * vectorOfTargets.Y - currentVectorY * vectorOfTargets.X;
            var scalarProduct = currentVectorX * vectorOfTargets.X + currentVectorY * vectorOfTargets.Y;
            var newAngle = Math.Acos(scalarProduct / (Distance(0, 0, currentVectorX, currentVectorY) * Distance(0, 0, vectorOfTargets.X, vectorOfTargets.Y)));
            vectorProduct = vectorProduct > 0 ? 1 : -1;
            return newAngle * vectorProduct;
        }
    }

    public class MyPoint
    {
        public int X;
        public int Y;

        public MyPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode()*2+Y.GetHashCode()*3;
        }

        public override bool Equals(object obj)
        {
            var p = obj as MyPoint;
            return p.X == X && p.Y == Y;
        }

        public override string ToString()
        {
            return string.Format("X = {0}, Y = {1}", X*26, Y*26);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryLibrary
{
    public class MyPoint
    {
        public float X;
        public float Y;

        public bool IsInSegment(Segment segment)
        {
            var coefs = Geometry.GetLineCoefs(segment);
            if (coefs[0] * X + coefs[1] * Y + coefs[2] != 0) return false;
            return Geometry.IsPointInRectangle(segment.StartX, segment.StartY, segment.EndX, segment.EndY, new MyPoint { X=X, Y=Y });
        }
    }

    public class Segment
    {
        public float StartX;
        public float StartY;
        public float EndX;
        public float EndY;

        public bool IsPointIn(MyPoint point)
        {
            var coefs = Geometry.GetLineCoefs(new Segment { StartX = StartX, StartY = StartY, EndX = EndX, EndY = EndY });
            if (coefs[0] * point.X + coefs[1] * point.Y + coefs[2] != 0) return false;
            return Geometry.IsPointInRectangle(StartX, StartY, EndX, EndY, point);
        }
    }

    public class MyRectangle
    {
        public float Width;
        public float Height;
        public MyPoint Anchor;
        public float Slope;

        public MyRectangle Rotate(float angle, MyPoint axe)
        {
            var dist1 = Math.Sqrt((Anchor.X - axe.X) * (Anchor.X - axe.X) + (Anchor.Y - axe.Y) * (Anchor.Y - axe.Y));
            var ang1 = (Anchor.X - axe.X) / dist1;
            if (dist1 == 0) ang1 = 0;
            var anchor = new MyPoint { X = (float)(axe.X + dist1 * Math.Cos(Math.Acos(ang1) - angle)), Y = (float)(axe.Y - dist1 * Math.Sin(Math.Acos(ang1) - angle)) };
            return new MyRectangle { Anchor = anchor, Height = Height, Width = Width, Slope = Slope + angle };
        }

        public float[] GetCoords()
        {
            var x1 = (float)(Anchor.X + Width * Math.Cos(Slope));
            var y1 = (float)(Anchor.Y + Width * Math.Sin(Slope));
            var x2 = (float)(Anchor.X - Height * Math.Cos(Math.PI / 2 - Slope));
            var y2 = (float)(Anchor.Y + Height * Math.Sin(Math.PI / 2 - Slope));
            var x3 = (float)(x2 + Width * Math.Sin(Math.PI / 2 - Slope));
            var y3 = (float)(y2 + Width * Math.Cos(Math.PI / 2 - Slope));
            return new float[] { x1, y1, x2, y2, x3, y3 };
        }
    }

    public static class Geometry
    {
        public static Segment CreateSegment(MyPoint start, MyPoint end)
        {
            return new Segment { StartX = start.X, StartY = start.Y, EndX = end.X, EndY = end.Y };
        }

        public static bool IsPointInSegment(Segment segment, MyPoint point)
        {
            var coefs = GetLineCoefs(segment);
            if (coefs[0] * point.X + coefs[1] * point.Y + coefs[2] != 0) return false;
            return IsPointInRectangle(segment.StartX, segment.StartY, segment.EndX, segment.EndY, point);
        }

        public static bool IsPointInRectangle(double x1, double y1, double x2, double y2, MyPoint point)
        {
            if (point.X < Math.Min(x1, x2) || point.X > Math.Max(x1, x2)) return false;
            if (point.Y < Math.Min(y1, y2) || point.Y > Math.Max(y1, y2)) return false;
            return true;
        }

        public static double[] GetLineCoefs(Segment segment)
        {
            float a = segment.EndY - segment.StartY;
            float b = segment.StartX - segment.EndX;
            return new double[] { a, b, -a * segment.StartX - b * segment.StartY };
        }

        public static MyRectangle CreateRectangle(MyPoint a, MyPoint b, MyPoint c)
        {
            var ab=(float)Math.Sqrt((b.X-a.X) * (b.X-a.X) + (b.Y - a.Y) * (b.Y - a.Y));
            var ac = (float)Math.Sqrt((c.X - a.X) * (c.X - a.X) + (c.Y - a.Y) * (c.Y - a.Y));
            var bc = (float)Math.Sqrt((c.X - b.X) * (c.X - b.X) + (c.Y - b.Y) * (c.Y - b.Y));
            float hypotenuse = Math.Max(ab, Math.Max(ac, bc));
            var anchor = new MyPoint();
            if (hypotenuse > bc && hypotenuse > ac)
            {
                var props = GetProperties(bc, ac, b, c, a);
                return new MyRectangle { Width = props[0], Height = props[1], Slope = 0, Anchor = new MyPoint { X = props[2], Y = props[3] } };
            }
            if (hypotenuse > ac && hypotenuse > ab)
            {
                var props = GetProperties(ab, ac, b, a, c);
                return new MyRectangle { Width = props[0], Height = props[1], Slope = 0, Anchor = new MyPoint { X = props[2], Y = props[3] } };
            }
            if (hypotenuse > bc && hypotenuse > ab)
            {
                var props = GetProperties(ab, bc, a, b, c);
                return new MyRectangle { Width = props[0], Height = props[1], Slope = 0, Anchor = new MyPoint { X = props[2], Y = props[3] } };
            }
            throw new Exception("Wrong arguments");
        }

        public static float[] GetProperties(float s1, float s2, MyPoint p1, MyPoint p2, MyPoint p3)
        {
            var anchor = new MyPoint();
            float height = s2, width = s1;
            if (p2.X < p3.X && p2.Y < p1.Y || p2.X > p3.X && p2.Y > p1.Y || p2.X < p3.X && p2.Y > p1.Y || p2.X > p3.X && p2.Y < p1.Y)
            {
                var temp = height;
                height = width;
                width = temp;
            }
            if (p2.X > p3.X && p2.Y < p1.Y || p2.X < p1.X && p2.Y > p3.Y) anchor = p3;
            else if (p2.X > p1.X && p2.Y < p3.Y || p2.X < p3.X && p2.Y > p1.Y) anchor = p1;
            else if (p2.X > p3.X && p2.Y > p1.Y || p2.X > p1.X && p2.Y > p3.Y) anchor = new MyPoint { X = p2.X - width, Y = p2.Y - height };
            else anchor = p2;
            return new float[] { width, height, anchor.X, anchor.Y };
        }
    }
}

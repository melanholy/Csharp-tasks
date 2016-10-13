using System;
using System.Drawing;

namespace Rectangles
{
    public class RectangleTasks
    {
        public bool AreIntersected(Rectangle r1, Rectangle r2)
        {
            return r1.X <= r2.Width + r2.X && r1.X + r1.Width >= r2.X && r1.Y <= r2.Height + r2.Y && r1.Y + r1.Height >= r2.Y;
        }

        public int IntersectionSquare(Rectangle r1, Rectangle r2)
        {
            if (IndexOfInnerRectangle(r1, r2) != -1)
                return Math.Min(r1.Width * r1.Height, r2.Width * r2.Height);
            if (AreIntersected(r1, r2))
                return (r1.Width - (Math.Max(r1.X, r2.X) - Math.Min(r1.X, r2.X))) * (Math.Min(r1.Height, r2.Height) - (Math.Max(r1.Y, r2.Y) - Math.Min(r1.Y, r2.Y))); 
            return 0;
        }

        public int IndexOfInnerRectangle(Rectangle r1, Rectangle r2)
        {
            bool oneIsInner = r1.X <= r2.X && r1.Y <= r2.Y && r1.X + r1.Width >= r2.X + r2.Width && r1.Y + r1.Height >= r2.Y + r2.Height;
            bool twoIsInner = r2.X <= r1.X && r2.Y <= r1.Y && r2.X + r2.Width >= r1.X + r1.Width && r2.Y + r2.Height >= r1.Y + r1.Height;
            if (oneIsInner || twoIsInner)
                return r1.Width * r1.Height >= r2.Width * r2.Height? 1 : 0;
            return -1;
        }
    }
}
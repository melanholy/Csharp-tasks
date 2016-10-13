using System;
using System.Drawing;

namespace Rectangles
{
    public class RectangleTasks
    {
        public bool AreIntersected(Rectangle r1, Rectangle r2)
        {
            return (r1.X + r1.Width >= r2.X) && (r1.X <= r2.Width + r2.X) && (r1.Y + r1.Height >= r2.Y) && (r1.Y <= r2.Height + r2.Y);
        }

        public int IntersectionSquare(Rectangle r1, Rectangle r2)
        {
            if (AreIntersected(r1, r2))
                if (IndexOfInnerRectangle(r1, r2) != -1)
                    return Math.Min(r1.Height * r1.Width, r2.Height * r2.Width);
                else return (r1.Width - (Math.Max(r1.X, r2.X) - Math.Min(r1.X, r2.X))) * (Math.Min(r1.Height, r2.Height) - (Math.Abs(r1.Y - r2.Y)));
            else return 0;
        }

        public int IndexOfInnerRectangle(Rectangle r1, Rectangle r2)
        {
            if (AreIntersected(r1,r2) && r1.X<r2.X && r1.Y < r2.Y && r1.X+r1.Weight) 
            return -1;
        }
    }
}
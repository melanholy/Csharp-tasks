using System;
using System.Drawing;

namespace WindowsFormsApplication1.Objects
{
    public class Bullet : IMovable
    {
        public Point Position { get; set; }
        public string Image => "Bullet.png";
        public int DrawingPriority => 1;
        public double Angle { get; }
        private const int Speed = 20;

        public Bullet(int x, int y, double angle)
        {
            Angle = angle;
            Position = new Point(x, y);
        }

        public void Move()
        {
            Position = new Point((int)(Position.X + Speed * Math.Cos(Angle)), (int)(Position.Y + Speed * Math.Sin(Angle)));
        }
    }
}

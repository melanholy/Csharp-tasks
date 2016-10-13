using System;
using System.Drawing;

namespace WindowsFormsApplication1.Objects
{
    public class Cursor : IMovable
    {
        public Point Position { get; set; }
        public string Image => "Cursor.png";
        public Game Game { get; }
        public int DrawingPriority => 0;
        public double Angle { get; private set; }
        public event Action<int, int> CursorMove;

        public Cursor(int x, int y, Game game)
        {
            Game = game;
            Position = new Point(x, y);
        }

        public void Move()
        {
            var deltaX = Game.DeltaX;
            var deltaY = Game.DeltaY;
            if (deltaX != 0 && deltaY != 0)
            {
                deltaY = (int)(deltaY / Math.Sqrt(2));
                deltaY += deltaY > 0 ? 1 : -1;
                deltaX = (int)(deltaX / Math.Sqrt(2));
                deltaX += deltaX > 0 ? 1 : -1;
            }
            Position = new Point(Position.X + deltaX, Position.Y + deltaY);
            if (CursorMove != null && (deltaX != 0 || deltaY != 0)) CursorMove(deltaX, deltaY);
        }
    }
}
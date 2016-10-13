using System.Drawing;

namespace WindowsFormsApplication1.Objects
{
    public class VWall : IGameObject
    {
        public Point Position { get; set; }
        public string Image => "VerticalWall.png";

        public VWall(int x, int y)
        {
            Position = new Point(x, y);
        }
    }

    public class HWall : IGameObject
    {
        public string Image => "HorizontalWall.png";
        public Point Position { get; set; }

        public HWall(int x, int y)
        {
            Position = new Point(x, y);
        }
    }
}

using System.Drawing;

namespace WindowsFormsApplication1.Objects
{
    public class Knife : IGameObject
    {
        public Point Position { get; set; }
        public string Image => "Knife.png";

        public Knife(int x, int y)
        {
            Position = new Point(x, y);
        }
    }
}

using System.Drawing;

namespace WindowsFormsApplication1.Objects
{
    public class Gun : IGameObject
    {
        public Point Position { get; set; }
        public string Image => "Gun.png";
        public int Bullets;

        public Gun(int x, int y, int shots)
        {
            Bullets = shots;
            Position = new Point(x, y);
        }
    }
}

using System.Drawing;

namespace WindowsFormsApplication1.Objects
{
    public class Floor : IGameObject
    {
        public string Image { get; }
        public Point Position { get; set; }

        public Floor(int x, int y, string image)
        {
            Image = image;
            Position = new Point(x, y);
        }
    }
}

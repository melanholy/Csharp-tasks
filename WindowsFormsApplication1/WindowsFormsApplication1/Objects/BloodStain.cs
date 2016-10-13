using System.Drawing;

namespace WindowsFormsApplication1.Objects
{
    public class BloodStain : IGameObject
    {
        public string Image => "BloodStain.png";
        public Point Position { get; set; }
        public BloodStain(int x, int y)
        {
            Position = new Point(x, y);
        }
    }
}

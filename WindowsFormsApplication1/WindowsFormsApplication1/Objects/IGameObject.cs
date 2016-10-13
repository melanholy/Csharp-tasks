using System.Drawing;

namespace WindowsFormsApplication1.Objects
{
    public interface IGameObject
    {
        string Image { get; }
        Point Position { get; set; }
    }
}

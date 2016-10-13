namespace WindowsFormsApplication1.Objects
{
    public interface IMovable : IGameObject
    {
        int DrawingPriority { get; }
        void Move();
        double Angle { get; }
    }
}

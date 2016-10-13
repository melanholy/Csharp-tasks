namespace WindowsFormsApplication2
{
    public interface ITemporable : IHaveCost
    {
        double Duration { get; set; }
    }
}
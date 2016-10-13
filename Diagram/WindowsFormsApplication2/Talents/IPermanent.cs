namespace WindowsFormsApplication2
{
    public interface IPermanent : IHaveCost
    {
        bool DispelAfterDeath { get; set; }
    }
}
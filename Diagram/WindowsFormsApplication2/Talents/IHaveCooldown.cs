namespace WindowsFormsApplication2
{
    public interface IHaveCooldown : ITalent, IHaveCost
    {
        double Cooldown { get; set; }
    }
}
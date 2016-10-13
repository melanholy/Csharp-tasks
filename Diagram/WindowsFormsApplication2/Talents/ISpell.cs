namespace WindowsFormsApplication2
{
    public interface ISpell : ICastable, IMageTalent
    {
        double CastTime { get; set; }
        int Range { get; set; }
    }
}
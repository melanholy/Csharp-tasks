namespace WindowsFormsApplication2
{
    public interface IMage : ICharacter
    {
        int ManaPool { get; set; }
        double MagicShield { get; set; }
        int Intellect { get; set; }
        IMageTalent[] Talents { get; set; }
    }
}
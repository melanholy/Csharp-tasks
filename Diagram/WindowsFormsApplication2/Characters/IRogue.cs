namespace WindowsFormsApplication2
{
    public interface IRogue : ICharacter
    {
        int EnergyPool { get; set; }
        int Dexterity { get; set; }
        int DodgeChance { get; set; }
        IRogueTalent[] Talents { get; set; }
    }
}
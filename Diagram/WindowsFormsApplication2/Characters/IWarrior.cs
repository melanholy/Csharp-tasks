namespace WindowsFormsApplication2
{
    public interface IWarrior : ICharacter
    {
        int FuryPool { get; set; }
        double Armor { get; set; }
        int Strength { get; set; }
        IWarriorTalent[] Talents { get; set; }
    }
}
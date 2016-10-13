namespace WindowsFormsApplication2
{
    public interface ICastable : IHaveCost
    {
        void Cast(ICharacter[] targets);
    }
}
namespace IDStore
{
    public interface ITuple<out T>
    {
        int E1 { get; }
        T E2 { get; }
    }
}

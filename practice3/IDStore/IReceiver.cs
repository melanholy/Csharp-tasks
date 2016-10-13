namespace IDStore
{
    public interface IReceiver<in T>
    {
        void Save(T obj);
    }
}
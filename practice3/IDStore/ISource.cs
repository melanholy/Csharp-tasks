namespace IDStore
{
    public interface ISource<out T>
    {
        T GetById(int id);
        ITuple<T>[] GetAll();
    }
}
namespace IDStore
{
    public class Source<T> : ISource<T>
    {
        private readonly ITuple<T>[] Storage;

        public Source(ITuple<T>[] storage)
        {
            Storage = storage;
        }

        public T GetById(int id)
        {
            return Storage[id].E2;
        }

        public ITuple<T>[] GetAll()
        {
            return Storage;
        }
    }
}
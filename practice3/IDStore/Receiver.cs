namespace IDStore
{
    public class Receiver<T> : IReceiver<T>
    {
        private readonly ITuple<T>[] Storage;
        private int Index;

        public Receiver(ITuple<T>[] storage)
        {
            Index = 0;
            Storage = storage;
        }

        public void Save(T obj)
        {
            var a = new MyTuple<T>(Index, obj);
            Storage[Index] = a;
            Index++;
        }
    }
}
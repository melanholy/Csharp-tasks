namespace IDStore
{
    public class MyTuple<T> : ITuple<T>
    {
        public int E1 { get; }
        public T E2 { get; }

        public MyTuple(int e1, T e2)
        {
            E1 = e1;
            E2 = e2;
        }
    }
}
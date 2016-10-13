namespace IDStore
{
    class Program
    {
        static void Main()
        {
            var s = new Storage();
            s.AddObject<Program>();
            s.AddObject<B>();
            s.AddObject<A>();

            var a = s.GetObjectsByClass<Program>();
            var b = s.GetObjectById<Program>(a[0].E1);
            var g = new Program();
        }
    }

    public class A { }

    public class B : A { }

    public interface II<in T>
    {
        void DoStuff(T obj);
    }

    public class R<T> : II<T>
    {
        public void DoStuff(T obj) { }
    }
}

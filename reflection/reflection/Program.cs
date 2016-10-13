using System;

namespace reflection
{

    static class Program
    {
        [STAThread]
        static void Main()
        {
            var a = Differentiator.Differentiate(x => x*x*x*x*x);
            Console.WriteLine($"{a(12.0)}, {5*Math.Pow(12, 4)}");
            Console.ReadKey();
        }
    }
}

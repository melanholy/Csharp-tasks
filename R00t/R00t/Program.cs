using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R00t
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args[0] == args[1])
            {
                Console.WriteLine("Invalid arguments.");
            }
            else
            {
                var answer = FindRoot(args);
                Console.WriteLine(answer);
            }
            Console.ReadKey();
        }

        public static double FindRoot(string[] args)
        {
            var x = double.Parse(args[1]);
            var b = double.Parse(args[1]);
            var en = Math.Abs(double.Parse(args[0]) - double.Parse(args[1]));
            while (Math.Abs(en) > 1e-6)
            {
                double pr = 0.0, f = 0.0;
                for (var i = 1; i < args.Length - 1; i++)
                {
                    pr += double.Parse(args[i + 1]) * (i - 1) * Math.Pow(x, i - 2);
                    f += double.Parse(args[i + 1]) * Math.Pow(x, i - 1);
                }
                x -= (f / pr);
                en = Math.Abs(x - b);
                b = x;
            }
            return x;
        }
    }
}

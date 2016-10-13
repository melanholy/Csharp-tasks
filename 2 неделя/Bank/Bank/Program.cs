using System;

namespace Bank
{
    class Program
    {   
        static double Calculations(double sum, double percent, double time)
        {
            return Math.Round(sum * (Math.Pow(1 + percent / 1200, time)), 2);
        }

        static void Main()
        {
            double sum = double.Parse(Console.ReadLine());
            double percent = double.Parse(Console.ReadLine());
            double time = double.Parse(Console.ReadLine());
            Console.WriteLine(Calculations(sum, percent, time));
            Console.ReadKey();
        }
    }
}

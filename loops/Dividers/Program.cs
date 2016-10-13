using System;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                long number = 0, k = 1;
                string input = Console.ReadLine();
                if (input == "") break;
                else number = long.Parse(input);
                for (int div = 1; div <= number / 2; div++)
                {
                    if (number % div == 0)
                    {
                        Console.Write(div + "(");
                        if (div != 1)
                            for (int i = 1; Math.Pow(div, i - 1) <= number / 2; i++)
                                if (number % Math.Pow(div, i) == 0) k = i;
                    }
                    if (div != 1 && number % div == 0) Console.Write(k + "), ");
                    else if (div == 1) Console.Write("infinity), ");
                }
                Console.WriteLine(number + "(1)");
            }
        }
    }
}

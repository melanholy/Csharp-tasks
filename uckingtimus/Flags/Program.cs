using System;
using System.Collections.Generic;

namespace Flags
{
    public static class FibonacciSequence
    {
        private static readonly List<long> FibonacciNumbers = new List<long>
        {
            1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377, 610, 987, 
            1597, 2584, 4181, 6765, 10946, 17711, 28657, 46368, 75025, 121393, 
            196418, 317811, 514229, 832040, 1346269, 2178309, 3524578, 5702887, 
            9227465, 14930352, 24157817, 39088169, 63245986, 102334155, 165580141, 
            267914296, 433494437, 701408733, 1134903170, 2269806340
        };

        public static long Fibonacci(int n)
        {
            while (FibonacciNumbers.Count < n)
                FibonacciNumbers.Add(FibonacciNumbers[FibonacciNumbers.Count - 1] + FibonacciNumbers[FibonacciNumbers.Count - 2]);
            return FibonacciNumbers[n - 1];
        }
    }

    class Program
    {
        static void Main()
        {
            var stripesCount = int.Parse(Console.ReadLine());
            if (stripesCount == 1 || stripesCount == 2) Console.Write(2);
            else Console.Write(FibonacciSequence.Fibonacci(stripesCount) + FibonacciSequence.Fibonacci(stripesCount - 1) + FibonacciSequence.Fibonacci(stripesCount - 2));
            Console.ReadKey();
        }
    }
}

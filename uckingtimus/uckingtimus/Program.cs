using System;
using System.Numerics;

namespace Tickets
{
    public class Program
    {
        static BigInteger[,] cache;

        static BigInteger Calc(int n, int s)
        {
            if (cache[n, s] >= 0) return cache[n, s];
            if (n == 0) return s == 0 ? 1 : 0;
            cache[n, s] = 0;
            for (var i = 0; i < 10; i++)
                if (s - i >= 0) cache[n, s] += Calc(n - 1, s - i);
            return cache[n, s];
        }

        public static BigInteger GetLuckyTickets(int n, int s)
        {
            cache = new BigInteger[n + 1, s + 1];
            if (s % 2 != 0) return 0;
            var half = s / 2;
            for (var i = 0; i < n + 1; i++)
                for (var j = 0; j < half + 1; j++)
                    cache[i, j] = -1;
            return Calc(n, half) * Calc(n, half);
        }

        public static void Main()
        {
            var input = Console.ReadLine().Split(' ');
            Console.WriteLine(GetLuckyTickets(int.Parse(input[0]), int.Parse(input[1])));
        }
    }
}
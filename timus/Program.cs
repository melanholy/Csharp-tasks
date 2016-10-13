using System;
using System.Collections.Generic;
using System.Linq;

namespace монобиллиард
{
    class Program
    {
        static void Main(string[] args)
        {
            var amountOfStrikes = int.Parse(Console.ReadLine());
            var revisorsOrder = new Queue<int>();
            var rightOrder = new List<int>();
            rightOrder.Capacity = amountOfStrikes;
            var checkedStrikesCount = 0;
            var k = 1;
            revisorsOrder.Enqueue(int.Parse(Console.ReadLine()));
            rightOrder.Add(k);
            while (true)
                if (revisorsOrder.ElementAt(0) == rightOrder.ElementAt(rightOrder.Count - 1))
                {
                    revisorsOrder.Dequeue();
                    rightOrder.RemoveAt(rightOrder.Count - 1);
                    if (++checkedStrikesCount < amountOfStrikes - 1) revisorsOrder.Enqueue(int.Parse(Console.ReadLine()));
                    else break;
                    if (rightOrder.Count == 0) rightOrder.Add(++k);
                    if (rightOrder[rightOrder.Count - 1] > revisorsOrder.ElementAt(0))
                    {
                        Console.WriteLine("Cheater");
                        return;
                    }
                }
                else rightOrder.Add(++k);
            Console.WriteLine("Not a proof");
        }
    }
}
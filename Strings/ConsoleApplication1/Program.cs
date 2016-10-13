using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[][] input = Console.In.ReadToEnd().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Split( ' ' )).ToArray();

            for (var i = 1; i < input.GetLength(0);i++) {
                if (input[i][0] == "0") Console.WriteLine("sober sober");
                else Console.WriteLine("sober "+input[i][0]);
                var ip = new int[int.Parse(input[i][2])];
        }
            Console.ReadKey();
        }
    }
}

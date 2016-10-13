using System;
using System.Collections.Generic;

namespace CVS
{
    class Program
    {
        static void Main()
        {
            var clones = new List<Clone> { new Clone() };
            var commandsAmount = int.Parse(Console.ReadLine().Split(' ')[0]);
            for (var i = 0; i < commandsAmount; i++)
            {
                var command = Console.ReadLine().Split(' ');
                var cloneNumber = int.Parse(command[1]) - 1;
                switch (command[0])
                {
                    case "learn":
                        clones[cloneNumber].Learn(command[2]);
                        break;
                    case "rollback":
                        clones[cloneNumber].Rollback();
                        break;
                    case "relearn":
                        clones[cloneNumber].Relearn();
                        break;
                    case "clone":
                        clones[cloneNumber].Clone(clones);
                        break;
                    case "check":
                        clones[cloneNumber].Check();
                        break;
                }
            }
            Console.ReadKey();
        }
    }
}

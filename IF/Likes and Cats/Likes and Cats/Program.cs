using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Likes_and_Cats
{
    class Program
    {
        static string Declension(string word, int numeral)
        {

            if (numeral == 1) return numeral + " " + word;
            if (numeral % 100 < 21 && numeral % 100 > 4) return numeral + " " + word + "ов";
            if (numeral % 10 < 5 && numeral % 10 != 0 && numeral % 10 != 1) return numeral + " " + word + "а";
            return numeral % 10 == 1 ? numeral + " " + word : numeral + " " + word + "ов";
        }

        static void Main()
        {
            while (true)
            {
                string numeral = Console.ReadLine();
                if (numeral == "") break;
                string word = Console.ReadLine();
                Console.WriteLine(Declension(word, int.Parse(numeral)));
            }
        }
    }
}

using System;
using System.IO;

namespace Pluralize
{
	static class Program
	{
		static void Main(string[] args)
		{
			string[] lines = File.ReadAllLines("rubles.txt");
			bool hasErrors = false;
			foreach (var line in lines)
			{
				string[] words = line.Split(' ');
				int count = int.Parse(words[0]);
				string rightAnswer = words[1];
				string pluralizedRubles = PluralizeRubles(count);
				if (pluralizedRubles != rightAnswer)
				{
					hasErrors = true;
					Console.WriteLine("Wrong answer: {0} {1}", count, pluralizedRubles);
				}
			}
            if (!hasErrors)
				Console.WriteLine("Correct!");
            Console.ReadKey();
		}

		private static string PluralizeRubles(int count)
		{
            if (count == 1) return "рубль";
            if (count % 100 < 21 && count % 100 > 4) return "рублей";
            if (count % 10 < 5 && count % 10 != 0 && count % 10 != 1) return "рубля";
            return count % 10 == 1?  "рубль" : "рублей";
		}
	}
}

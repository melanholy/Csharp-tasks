using System;
using System.IO;
using System.Linq;
using System.Text;

namespace names
{
	public class Program
	{
		private const string dataFilePath = @"..\..\names.txt";

		private static void Main(string[] args)
		{
			NameData[] namesData = ReadData();
			NamesTasks.ShowBirthDayMonthStatistics(namesData);
			Console.WriteLine();
            NamesTasks.ShowBirthDayMonthStatisticsForName(namesData, "виктория");
            Console.WriteLine();
            NamesTasks.ShowBirthDayMonthStatisticsForName(namesData, "юрий");
            Console.WriteLine();
            NamesTasks.ShowBirthDayMonthStatisticsForName(namesData, "илья");
            Console.WriteLine();
            NamesTasks.ShowBirthDayMonthStatisticsForName(namesData, "владимир");
            Console.WriteLine();
			NamesTasks.ShowBirthYearsStatisticsHistogram(namesData);
			Console.WriteLine();
		}


		private static NameData[] ReadData()
		{
			string[] lines = File.ReadAllLines(dataFilePath, Encoding.GetEncoding(1251));
			var names = new NameData[lines.Length];
			for (int i = 0; i < lines.Length; i++)
				names[i] = NameData.ParseFrom(lines[i]);
			return names;
		}

		private static NameData[] ReadData2()
		{
			return File.ReadLines(dataFilePath).Select(NameData.ParseFrom).ToArray();
		}

	}
}

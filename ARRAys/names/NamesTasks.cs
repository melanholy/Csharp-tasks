using System;
using System.Collections.Generic;
using System.Linq;

namespace names
{
	class NamesTasks
	{
		public static void ShowBirthDayMonthStatistics(NameData[] names)
		{
			Console.WriteLine("Статистика рождаемости по датам");
            int notFirstAndNotLast = 0, last = 0, first = 0;
            foreach (var name in names) 
            {
                int day = name.BirthDate.Day, month = name.BirthDate.Month;
                if (DateTime.DaysInMonth(2001, month)==day) last++;
                else if (day == 1) first++;
                else notFirstAndNotLast++;
            }
            Console.WriteLine("Количество людей, рожденных в первый день месяца: "+first);
            Console.WriteLine("Количество людей, рожденных в последний день месяца: "+last);
            Console.WriteLine("Усредненное по всем остальным дням месяца количество рожденных людей: " + Math.Round(notFirstAndNotLast/341.00, 2));
		}

		public static void ShowBirthYearsStatisticsHistogram(NameData[] names)
		{
            var rawData = new Dictionary<int, int>();
            foreach (var name in names)
            {
                if (!rawData.ContainsKey(name.BirthDate.Year)) rawData[name.BirthDate.Year] = 0;
                rawData[name.BirthDate.Year]++;
            }
            Histogram.Show("Рождаемость по годам", rawData.Keys.ToArray(), rawData.Values.ToArray());
		}

		public static void ShowBirthDayMonthStatisticsForName(NameData[] names, string name)
		{
			Console.WriteLine("Статистика рождаемости имени {0}", name);
            var dates = new int[32,13];
            var counter = 0;
            //var u = new string[names.Length];
            foreach (var person in names)
                //u[i]=i.BirthDate.Day.ToString()+i.BirthDate.Month.ToString();
                if (person.Name == name)
                {
                    dates[person.BirthDate.Day, person.BirthDate.Month]++;
                    counter++;
                }
            var maxCount = 0;
            var neededData = "";
            for (int i=1;i<32;i++)
                for (int j=1;j<13;j++)
                    if (dates[i,j]>maxCount)
                    {
                        maxCount = dates[i, j];
                        neededData = i.ToString() + '.' + j.ToString();
                    }
            Console.WriteLine("Больше всего людей с именем {0} родилось {1} ", name, neededData);
            //Console.WriteLine("Это {0}% от всех людей родившихся в эту дату", Math.Round((double)m / u.Count(x => x == s.Split('.')[0] + s.Split('.')[1]) * 100, 2));
            Console.WriteLine("Процент людей с именем {0}, рожденных в эту дату: {1}", name, Math.Round((double)maxCount / counter * 100, 2));
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    public static class IEnumerableExtensions
    {
        public static double Median(this IEnumerable<double> items)
        {
            items = items.OrderBy(x => x);
            var count = items.Count();
            if (count % 2 == 1) return items.ElementAt(count / 2);
            return (items.ElementAt((count - 1) / 2) + items.ElementAt((count - 1) / 2 + 1)) / 2;
        }

        public static IEnumerable<Tuple<T, T>> GetBigrams<T>(this IEnumerable<T> items)
        {
            var tuples = new List<Tuple<T, T>>();
            for (var i = 0; i < items.Count() - 1; i++)
                tuples.Add(new Tuple<T, T>(items.ElementAt(i), items.ElementAt(i + 1)));
            return tuples;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var a = new List<double> { 400, 250, 640, 700, 900, 100, 300, 170, 550 };
            Console.WriteLine(a.Median());
            var bigrams = a.GetBigrams();
            foreach (var e in bigrams)
                Console.WriteLine(e);
            var slideFile = File.ReadAllLines("slides.txt");
            var visitsFile = File.ReadAllLines("visits.txt");
            var slides = slideFile.ToDictionary(x => x.Split(';')[0], y => y.Split(';')[1]);
            var users = visitsFile.ToLookup(x => x.Split(';')[0], y => Tuple.Create(y.Split(';')[1], y.Split(';')[3], y.Split(';')[2]));
            var dict = new Dictionary<string, List<double>>();
            dict["exercise"] = new List<double>();
            dict["theory"] = new List<double>();
            dict["quiz"] = new List<double>();
            var usersVisits = users.SelectMany(e => e.GetBigrams()
                                   .ToLookup(w => slides[w.Item1.Item1], d => GetDateTime(d.Item2.Item2, d.Item2.Item3, d.Item1.Item2, d.Item1.Item3)));
            foreach (var lookup in usersVisits)
                dict[lookup.Key] = dict[lookup.Key].Concat(lookup).Where(x => x > 1 && x < 120).ToList();
            Console.WriteLine("{0} minutes per theory slide.", dict["theory"].Median());
            Console.WriteLine("{0} minutes per exercise slide.", dict["exercise"].Median());
            Console.WriteLine("{0} minutes per quiz slide.", dict["quiz"].Median());
            Console.ReadKey();
        }

        public static double GetDateTime(string time1, string date1, string time2, string date2)
        {
            var partedTime1 = time1.Split(':', '.').Select(int.Parse).ToArray();
            var partedDate1 = date1.Split('-').Select(int.Parse).ToArray();
            var partedTime2 = time2.Split(':', '.').Select(int.Parse).ToArray();
            var partedDate2 = date2.Split('-').Select(int.Parse).ToArray();
            var span1 = new DateTime(partedDate1[0], partedDate1[1], partedDate1[2], partedTime1[0], partedTime1[1], partedTime1[2], partedTime1[3]);
            var span2 = new DateTime(partedDate2[0], partedDate2[1], partedDate2[2], partedTime2[0], partedTime2[1], partedTime2[2], partedTime2[3]);
            return (span1 - span2).TotalMinutes;
        }
    }
}

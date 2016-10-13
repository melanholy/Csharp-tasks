using System;
using System.Collections.Generic;
using System.IO;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public static class IEnumerableExtensions
    {
        public static List<T> ToList<T>(this IEnumerable<T> items)
        {
            var itemsList = new List<T>();
            var enumerator = items.GetEnumerator();
            while (enumerator.MoveNext())
                itemsList.Add(enumerator.Current);
            return itemsList;
        }

        public static double Median(this IEnumerable<double> items)
        {
            var itemsList = items.ToList();
            for (var i = 0; i < itemsList.Count - 1; i++)
                for (var j = 0; j < itemsList.Count - i - 1; j++)
                    if (itemsList[j] > itemsList[j + 1])
                    {
                        var temp = itemsList[j];
                        itemsList[j] = itemsList[j + 1];
                        itemsList[j + 1] = temp;
                    }
            if (itemsList.Count % 2 == 1) return itemsList[itemsList.Count / 2];
            return (itemsList[itemsList.Count / 2] + itemsList[itemsList.Count / 2 + 1]) / 2;
        }

        public static IEnumerable<Tuple<T, T>> GetBigrams<T>(this IEnumerable<T> items)
        {
            var itemsList = items.ToList();
            var tuples = new List<Tuple<T, T>>();
            for (var i = 0; i < itemsList.Count - 1; i++)
                tuples.Add(new Tuple<T, T>(itemsList[i], itemsList[i + 1]));
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
            var slides = new Dictionary<string, string>();
            foreach (var line in slideFile)
            {
                var partedLine = line.Split(';');
                slides[partedLine[0]] = partedLine[1];
            }
            var users = new Dictionary<string, List<Tuple<string, string, string>>>();
            foreach (var line in visitsFile)
            {
                var partedLine = line.Split(';');
                if (!users.ContainsKey(partedLine[0]))
                    users[partedLine[0]] = new List<Tuple<string, string, string>>();
                users[partedLine[0]].Add(Tuple.Create(partedLine[1], partedLine[3], partedLine[2]));
            }
            var exercises = new List<double>();
            var theory = new List<double>();
            var quizes = new List<double>();
            foreach (var e in users)
                foreach(var d in e.Value.GetBigrams())
                {
                    var time = GetDateTime(d.Item2.Item2, d.Item2.Item3, d.Item1.Item2, d.Item1.Item3);
                    if (time > 1 && time < 120)
                        if (slides[d.Item1.Item1] == "exercise") exercises.Add(time);
                        else if (slides[d.Item1.Item1] == "theory") theory.Add(time);
                        else if (slides[d.Item1.Item1] == "quiz") quizes.Add(time);
                }
            Console.WriteLine("{0} minutes per theory slide.", theory.Median());
            Console.WriteLine("{0} minutes per exercise slide.", exercises.Median());
            Console.WriteLine("{0} minutes per quiz slide.", quizes.Median());
            Console.ReadKey();
        }

        public static double GetDateTime(string time1, string date1, string time2, string date2)
        {
            var partedTime1 = time1.Split(':', '.');
            var partedDate1 = date1.Split('-');
            var partedTime2 = time2.Split(':', '.');
            var partedDate2 = date2.Split('-');
            var span1 = new DateTime(int.Parse(partedDate1[0]), int.Parse(partedDate1[1]), int.Parse(partedDate1[2]), int.Parse(partedTime1[0]), 
                                     int.Parse(partedTime1[1]), int.Parse(partedTime1[2]), int.Parse(partedTime1[3]));
            var span2 = new DateTime(int.Parse(partedDate2[0]), int.Parse(partedDate2[1]), int.Parse(partedDate2[2]), int.Parse(partedTime2[0]), 
                                     int.Parse(partedTime2[1]), int.Parse(partedTime2[2]), int.Parse(partedTime2[3]));
            return (span1 - span2).TotalMinutes;
        }
    }
}

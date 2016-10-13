using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

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
            var dict = new Dictionary<string, List<int>>();
            dict["exercise"] = new List<int>();
            dict["theory"] = new List<int>();
            dict["quiz"] = new List<int>();
            var usersVisits = users.SelectMany(x => x.ToLookup(w => slides[w.Item1], y => GetDateTime(y.Item2)));
            foreach (var lookup in usersVisits)
                dict[lookup.Key] = dict[lookup.Key].Concat(lookup).ToList();
            var chart = new Chart();
            chart.ChartAreas.Add(new ChartArea());
            chart.Series.Add(BuildGraph(dict["theory"]));
            chart.Series.Add(BuildGraph(dict["exercise"]));
            chart.Series.Add(BuildGraph(dict["quiz"]));
            var form = new Form
            {
                WindowState = FormWindowState.Maximized,
                Text = "Синий - теория, жёлтый - задачи, красный - тесты "
            };
            chart.Dock = DockStyle.Fill;
            form.Controls.Add(chart);
            Application.Run(form);
        }

        public static Series BuildGraph(List<int> a)
        {
            var series = new Series() { ChartType = SeriesChartType.Column };
            for (var x = 0; x < 25; x++)
                series.Points.Add(new DataPoint(x, a.Count(w => w == x)));
            return series;
        }

        public static int GetDateTime(string time)
        {
            var partedTime1 = time.Split(':', '.').Select(int.Parse).ToArray();
            return new DateTime(2014, 3, 21, partedTime1[0], partedTime1[1], partedTime1[2], partedTime1[3]).Hour;
        }
    }
}

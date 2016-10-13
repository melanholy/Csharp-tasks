using System;
using System.Collections.Generic;
using System.IO;
//using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApplication1
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
            itemsList.BubbleSort();
            if (itemsList.Count % 2 == 1) return itemsList[itemsList.Count / 2];
            return (itemsList[itemsList.Count / 2] + itemsList[itemsList.Count / 2 + 1]) / 2;
        }

        public static void BubbleSort<T>(this List<T> itemsList) where T: IComparable
        {
            for (var i = 0; i < itemsList.Count - 1; i++)
                for (var j = 0; j < itemsList.Count - i - 1; j++)
                    if (itemsList[j].CompareTo(itemsList[j + 1]) > 0)
                    {
                        var temp = itemsList[j];
                        itemsList[j] = itemsList[j + 1];
                        itemsList[j + 1] = temp;
                    }
        }

        public static int Count<T>(this List<T> itemsList, Func<T, bool> predicate)
        {
            var k = 0;
            foreach(var e in itemsList)
                if (predicate(e)) k++;
            return k;
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

    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
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
            var exercises = new List<int>();
            var theory = new List<int>();
            var quizes = new List<int>();
            foreach (var e in users)
                foreach (var d in e.Value)
                {
                    var time = GetDateTime(d.Item2);
                    if (slides[d.Item1] == "exercise") exercises.Add(time);
                    else if (slides[d.Item1] == "theory") theory.Add(time);
                    else if (slides[d.Item1] == "quiz") quizes.Add(time);
                }
            var chart = new Chart();
            chart.ChartAreas.Add(new ChartArea());
            chart.Series.Add(BuildGraph(theory));
            chart.Series.Add(BuildGraph(exercises));
            chart.Series.Add(BuildGraph(quizes));
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
            var partedTime1 = time.Split(':', '.');
            return new DateTime(2014, 3, 20, int.Parse(partedTime1[0]), int.Parse(partedTime1[1]), int.Parse(partedTime1[2]), int.Parse(partedTime1[3])).Hour;
        }
    }
}

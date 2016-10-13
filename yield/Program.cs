using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace yield
{
	class Program
	{
		private readonly Form form;
		private readonly Chart chart;
		private readonly Series original;
		private readonly Series exp;
		private readonly Series avg;
		private volatile bool paused;
		private volatile bool canceled;
		private Thread thread;

		public Program()
		{
			form = new Form
			{
				WindowState = FormWindowState.Maximized,
				Text = "Click to pause / resume"
			};
			chart = new Chart
			{
				Dock = DockStyle.Fill
			};
			chart.ChartAreas.Add(new ChartArea());
			original = AddSeries("original");
			avg = AddSeries("avg");
			exp = AddSeries("exp");
			form.Controls.Add(chart);
			chart.Click += (sender, args) => paused = !paused;
			form.FormClosing += (sender, args) =>
			{
				canceled = true;
				thread.Join();
			};
		}

		[STAThread]
		static void Main()
		{
			new Program().Run();
		}

		private void Run()
		{
			RunDataTask();
			var folderToAnalyze = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			//RunExtensionsSizeTask(folderToAnalyze, 10000);
		}

		private void RunDataTask()
		{
			thread = new Thread(() =>
			{
				foreach (var p in DataTask.GetData(new Random()))
				{
					if (canceled) return;
					form.BeginInvoke((Action) (() => AddPoint(p)));
					while (paused) Thread.Sleep(50);
					Thread.Sleep(50);
				}
			}) {IsBackground = true};
			thread.Start();
			form.ShowDialog();
		}

		private static void RunExtensionsSizeTask(string path, int filesToAnalyze)
		{
			Console.WriteLine("Analyzing {1} files from '{0}' ...", path, filesToAnalyze);
			Console.WriteLine();
			var fileExtensionsRating =
				DirectoriesTask.EnumerateAllFiles(new DirectoryInfo(path), new Random())
					.Take(filesToAnalyze)
					.GroupBy(t => t.FileExtension.ToLower(), t => t.Size, (ext, g) => new { ext, Size = g.Sum() })
					.OrderByDescending(t => t.Size);

			Console.WriteLine("Mb\tFile extension");
			foreach (var extData in fileExtensionsRating.Take(10))
			{
				Console.WriteLine("{0}\t{1}", extData.Size/1024/1024, extData.ext);
			}
			Console.WriteLine("...");
		}

		private void AddPoint(DataPoint p)
		{
			original.Points.AddXY(p.X, p.OriginalY);
			avg.Points.AddXY(p.X, p.AvgSmoothedY);
			exp.Points.AddXY(p.X, p.ExpSmoothedY);
		}

		private Series AddSeries(string name)
		{
			var series = new Series(name, 130) { ChartType = SeriesChartType.Line, BorderWidth = 2 };

			chart.Series.Add(series);

			return series;
		}
	}
}

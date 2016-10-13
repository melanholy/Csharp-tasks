using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using TestingRoom;

namespace Distance
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new TestRoom(CreateTestCases()));
		}

		private static IEnumerable<TestCase> CreateTestCases()
		{
			return CreateTestCasesForLeftSide().Concat(CreateTestCasesForLeftSide().Select(d => new DistanceTestCase(-d.x, d.y, d.distance)));
		}

		private static IEnumerable<DistanceTestCase> CreateTestCasesForLeftSide()
		{
			yield return new DistanceTestCase(0, 0, 30*Math.Sqrt(2));
			yield return new DistanceTestCase(20, 20, 10*Math.Sqrt(2));
			yield return new DistanceTestCase(30, 40, 5*Math.Sqrt(2));
			yield return new DistanceTestCase(0, 60, 0);
			yield return new DistanceTestCase(0, 70, 10);
			yield return new DistanceTestCase(10, 70, 10 * Math.Sqrt(2));
			yield return new DistanceTestCase(40, 40, 10 * Math.Sqrt(2));
			yield return new DistanceTestCase(60, 20, 10 * Math.Sqrt(2));
			yield return new DistanceTestCase(62, 20, Math.Sqrt(244));
			yield return new DistanceTestCase(60, 12, Math.Sqrt(104));
			yield return new DistanceTestCase(60, 10, 10);
			yield return new DistanceTestCase(60, 0, 10);
			yield return new DistanceTestCase(60, -10, 10);
			yield return new DistanceTestCase(50, -10, 0);
			yield return new DistanceTestCase(50, 0, 0);
			yield return new DistanceTestCase(50, 10, 0);
			yield return new DistanceTestCase(40, 10, 5*Math.Sqrt(2));
			yield return new DistanceTestCase(40, 0, 10);
			yield return new DistanceTestCase(40, -10, 10);
			yield return new DistanceTestCase(40, -20, 50-Math.Sqrt(1700));
			yield return new DistanceTestCase(20, -30, 50-20*Math.Sqrt(2));
			yield return new DistanceTestCase(0, -30, 30);
			yield return new DistanceTestCase(0, -20, 40);
			yield return new DistanceTestCase(10, -10, 40);
			yield return new DistanceTestCase(-10, -10, 40);
			yield return new DistanceTestCase(0, -10, 35*Math.Sqrt(2));
		}
	}

	internal class DistanceTestCase : TestCase
	{
        int u = 0;
		public readonly int x;
		public readonly int y;
		public readonly double distance;
		private double answer;

		public DistanceTestCase(int x, int y, double distance) : base(string.Format("d({0},{1})", x, y))
		{
			this.x = x;
			this.y = y;
			this.distance = distance;
		}

		protected override void InternalVisualize(TestCaseUI ui)
		{ 
			ui.Line(-100, 0, 100, 0, neutralThinPen);
			ui.Line(0, -100, 0, 100, neutralThinPen);
			
			FigureShape.DrawFigure(ui);
			
			ui.Line(50, -10, 50, 10, neutralPen);
			ui.Line(-50, 10, 0, 60, neutralPen);
			ui.Line(50, 10, 0, 60, neutralPen);
			ui.Arc(0, -10, 50, 180, 180, neutralPen);

			ui.Circle(x, y, 1, neutralPen);
			ui.Circle(x, y, answer, new Pen(actualAnswerPen.Color, 1) {DashStyle = DashStyle.Custom, DashPattern = new float[]{4, 4}});
			ui.Circle(x, y, distance, new Pen(expectedAnswerPen.Color, 1) {DashStyle = DashStyle.Custom, DashPattern = new float[]{4, 4}});
			ui.Log("Point: ({0}, {1})", x, y);
			ui.Log("Calculated distance: {0}", answer);

		}

		protected override bool InternalRun()
		{
			answer = new DistanceTask().GetDistanceToCurve(x, y);
			return Math.Abs(answer - distance) < 1e-3;
		}
	}
}

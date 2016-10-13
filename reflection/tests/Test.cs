using NUnit.Framework;
using System;
using reflection;
using System.Linq.Expressions;

namespace tests
{
	[TestFixture]
	public class Test
	{
		private static readonly Random Rand = new Random ();

		private static void PerformTest(Func<double, double> expected, Expression<Func<double, double>> toDiff)
		{
			var diff = Differentiator.Differentiate (toDiff);
			for (var i = 0; i < 1000000; i++)
			{
				var x = Rand.NextDouble ();
				Assert.AreEqual (expected(x), diff(x));
			}
		}

		[Test]
		public void TestDiffMul ()
		{
			PerformTest (x => 4, x => 4 * x);
			PerformTest(x => 0, x => 4 * 4);
			PerformTest(x => 2*x, x => x * x);
			PerformTest (x => 96*x, x => x * 4 * x * 12);
		}

		[Test]
		public void TestDiffAdd ()
		{
			PerformTest (x => 1, x => 4 + x);
			PerformTest(x => 0, x => 4 + 4);
			PerformTest(x => 2, x => x + x);
			PerformTest (x => 2, x => x + 4 + x + 12);
		}

		[Test]
		public void TestDiffSin ()
		{
			PerformTest (Math.Cos, x => Math.Sin(x));
			PerformTest(x => 2 * Math.Cos(2 * x), x => Math.Sin(2*x));
			PerformTest(x => 2 * x * Math.Cos(x * x + 12), x => Math.Sin(x * x + 12));
		}
	}
}

using System;
using streams;
using System.Drawing;
using System.Drawing.Imaging;

namespace BitmapEditorPerfomanceTest
{
    internal class MainClass
	{
		public static void Main (string[] args)
		{
			var timer = new MyTimer ();
			var bmp = new Bitmap (800, 600, PixelFormat.Format24bppRgb);
			var rand = new Random ();

			var color = Color.FromArgb (2, 2, 8);
			using (timer.Start ())
				for (var i = 0; i < 10000000; i++)
					bmp.SetPixel(
						rand.Next(0, 799), rand.Next(0, 599),
						color
					);
			Console.WriteLine($"Set without lock: {timer.ElapsedMilliseconds}");
			timer.Reset();

			using (var be = new BitmapEditor(bmp))
			using (timer.Start ())
				for (var i = 0; i < 10000000; i++)
					be.SetPixel(
						rand.Next(0, 799), rand.Next(0, 599),
						2, 2, 8
					);
			Console.WriteLine($"Set with lock: {timer.ElapsedMilliseconds}");
			timer.Reset();

            bmp.Save("a.bmp");

			using (timer.Start ())
				for (var i = 0; i < 10000000; i++)
					bmp.GetPixel (rand.Next (0, 799), rand.Next (0, 599));
			Console.WriteLine ($"Get without lock: {timer.ElapsedMilliseconds}");
			timer.Reset();

			using (var be = new BitmapEditor(bmp))
			using (timer.Start ())
				for (var i = 0; i < 10000000; i++)
					be.GetPixel(rand.Next (0, 799), rand.Next (0, 599));
			Console.WriteLine ($"Get with lock: {timer.ElapsedMilliseconds}");
			timer.Reset();

		    Console.ReadKey();
		}
	}
}

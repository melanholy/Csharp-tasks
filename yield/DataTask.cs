using System;
using System.Collections;
using System.Collections.Generic;

namespace yield
{
	public class DataPoint
	{
		public double X;
		public double OriginalY;
		public double ExpSmoothedY;
		public double AvgSmoothedY;
	}
	
	public static class DataTask
	{
		public static IEnumerable<DataPoint> GetData(Random random)
		{
			return GenerateOriginalData(random).SmoothExponentialy(0.8).SmoothAverage(10);
		}

		public static IEnumerable<DataPoint> GenerateOriginalData(Random random)
		{
			double y;
			int x = 0;
			while (true)
			{
				x++;
				y = 10 * ((x / 50) % 2) + (random.NextDouble() - 0.5);
				yield return new DataPoint { X = x, OriginalY = y };
			}
		}

		public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double e)
		{
            var smoothY = 0.0;
            foreach (var point in data)
            {
                smoothY = e * point.OriginalY + (1 - e) * smoothY;
                yield return new DataPoint { X = point.X, OriginalY = point.OriginalY, ExpSmoothedY = smoothY };
            }
		}

		public static IEnumerable<DataPoint> SmoothAverage(this IEnumerable<DataPoint> data, int windowWidth)
		{
            var store = 0.0;
            var lastTenObservations = new Queue<double>();
            foreach(var point in data)
            {
                lastTenObservations.Enqueue(point.OriginalY);
                if (lastTenObservations.Count <= 10)
                {
                    foreach (var observation in lastTenObservations) store += observation;
                    store /= windowWidth;
                }
                else store = store - lastTenObservations.Dequeue() / windowWidth + point.OriginalY / windowWidth;
                point.AvgSmoothedY = store;
                yield return point;
            }
		}
	}
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace RoutePlanning
{
	public class PathFinder
	{
        static int k = 0;
		public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
		{
            var result = new List<int[]>();
            var bestOrder = new int[checkpoints.Length];
            MakeNotSoTrivialPermutation(checkpoints, result, bestOrder, 0);
			return result.Min();
		}

        public static void MakeNotSoTrivialPermutation(Point[] checkpoints, List<int[]> result, int[] bestOrder, int position = 0)
		{
            if (position == checkpoints.Length)
            {
                var ara = bestOrder.ToArray();
                result.Add(ara);
                return;
            }
            for (int i = 0; i < bestOrder.Length; i++)
            {
                var index = Array.IndexOf(bestOrder, i, 0, position);
                if (index == -1)
                    continue;
                bestOrder[position] = i;
                MakeNotSoTrivialPermutation(checkpoints, result, bestOrder, position + 1);
            }
		}

		public static double GetPathLen(Point[] checkpoints, int[] order)
		{
			Point prevPoint = checkpoints[0];
			var len = 0.0;
			foreach (int checkpointIndex in order)
			{
				len += Distance(prevPoint, checkpoints[checkpointIndex]);
				prevPoint = checkpoints[checkpointIndex];
			}
			return len;
		}

		public static double Distance(Point a, Point b)
		{
			var dx = a.X - b.X;
			var dy = a.Y - b.Y;
			return Math.Sqrt(dx * dx + dy * dy);
		}

        public static int[] FindBestCheckpointsOrder(Point[] checkpoints, TimeSpan timeout)
        {
            //Задача 4
            // Доделайте этот метод, чтобы он искал сначала рассматривал наиболее перспективные маршруты, и прекращал поиск через заданный timеout.

            var startTime = DateTime.Now;
            var bestFoundSolution = new int[10];
            //Ищем...
            //Если у нас кончилось время — возвращаем лучшее из того, что нашли.
            if (DateTime.Now - startTime > timeout) return bestFoundSolution;
            //Иначе, продолжаем искать.
            return bestFoundSolution;
        }
	}
}
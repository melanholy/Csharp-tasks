using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RoutePlanning
{
	public class PathFinder
	{
        static double minLength = 0;
        static int flag = 0;
		public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
		{
            var bestOrder = new int[checkpoints.Length];
            var result = new List<int[]>();
            minLength = 0;
            var startTime = DateTime.Now;
            MakeFirstSuggestion(checkpoints, result, bestOrder);
            bestOrder = new int[checkpoints.Length];
            if (checkpoints.Length < 15) MakeNotSoTrivialPermutation(checkpoints, startTime, TimeSpan.FromSeconds(2), result, bestOrder, 0);
            Console.WriteLine();
			return result[result.Count-1];
		}

        public static void MakeNotSoTrivialPermutation(Point[] checkpoints, DateTime startTime, TimeSpan timeout, List<int[]> result, int[] order, int position = 0)
        {
            if (position == checkpoints.Length)
            {
                var length = GetPathLen(checkpoints, order);
                if (minLength > length)
                {
                    minLength = length;
                    result.Add(order.ToArray());
                }
                return;
            }
            for (int i = 0; i < order.Length; i++) 
                if (Array.IndexOf(order, i, 0, position) == -1)
                {
                    var currentOrder = new int[position + 1];
                    for (var j = 0; j < position; j++)
                        currentOrder[j] = order[j];
                    order[position] = i;
                    currentOrder[position] = i;
                    if (minLength != 0 && GetPathLen(checkpoints, currentOrder) >= minLength) return;
                    MakeNotSoTrivialPermutation(checkpoints, startTime, timeout, result, order, position + 1);
                    if (DateTime.Now - startTime > timeout)
                    {
                        flag = 1;
                        return;
                    }
                    if (flag == 1) return;
                }
            return;
        }

        static void MakeFirstSuggestion(Point[] checkpoints, List<int[]> result, int[] order)
        {
            for (var i = 0; i < checkpoints.Length;i++ )
            {
                var currentOrder = new int[i + 1];
                for (var j = 0; j < i; j++)
                    currentOrder[j] = order[j];
                var bestNextCheck = 0;
                if (currentOrder.Length > 1) bestNextCheck = FindBestNextCheck(checkpoints, currentOrder);
                order[i] = bestNextCheck;
                currentOrder[i] = bestNextCheck;
            }
            var length = GetPathLen(checkpoints, order);
            minLength = length;
            result.Add(order.ToArray());
        }

        static int FindBestNextCheck(Point[] checkpoints, int[] order)
        {
            var allNumbers = MakeTrivialPermutation(checkpoints.Length);
            var availableNumbers = new List<int>();
            foreach (var i in allNumbers)
                if (!order.Contains(i)) availableNumbers.Add(i);
            var minStep = 0.0;
            var answer = 0;
            var nextStep = new int[2];
            nextStep[0] = order[order.Length - 2];
            foreach (var i in availableNumbers)
            {
                nextStep[1] = i;
                var currentStepLength = GetPathLen(checkpoints, nextStep);
                if (minStep == 0 || currentStepLength < minStep)
                {
                    answer = i;
                    minStep = currentStepLength;
                }
            }
            return answer;
        }

		public static int[] MakeTrivialPermutation(int size)
		{
			var bestOrder = new int[size];
			for (int i = 0; i < bestOrder.Length; i++)
				bestOrder[i] = i;
			return bestOrder;
		}

		public static double GetPathLen(Point[] checkpoints, int[] order)
		{
			Point prevPoint = checkpoints[0];
			var len = 0.0;
			foreach (int checkpointIndex in order)
			{
                var dx = (prevPoint.X - checkpoints[checkpointIndex].X);
                var dy = (prevPoint.Y - checkpoints[checkpointIndex].Y);
                len += Math.Sqrt(dx * dx + dy * dy);
				prevPoint = checkpoints[checkpointIndex];
			}
			return len;
		}
	}
}
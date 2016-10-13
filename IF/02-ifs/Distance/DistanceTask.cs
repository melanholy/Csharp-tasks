using System;

namespace Distance
{
	public class DistanceTask
	{
        const double bottom = 60;
        const double verticalAxis = 0;
        const double horizontalAxis = 0;
        const double leftSide = -50;
        const double rightSide = 50;

		public double GetDistanceToCurve(double x, double y)
		{
            if (y > horizontalAxis+10 && x >= rightSide+10) return Math.Sqrt((50 - x) * (50 - x) + (y - 10) * (y - 10));
            if (y > horizontalAxis+10 && x <= leftSide-10) return Math.Sqrt((x + 50) * (x + 50) + (y - 10) * (y - 10));
            if (x == verticalAxis && y >= bottom) return Math.Abs(60 - y);
            if (Math.Abs(x)<rightSide && y<x+60 && y<60-x && y>=horizontalAxis+10) return Math.Abs(Math.Abs(x) + Math.Abs(y) - 60) / Math.Sqrt(2);
            if (x != verticalAxis && y >= horizontalAxis-10 && y <= horizontalAxis+12) return Math.Abs(Math.Abs(x) - 50);
            if (x <= verticalAxis && (y >= horizontalAxis + 12 || y == horizontalAxis)) return Math.Abs(x - y + 60) / Math.Sqrt(2);
            if (x >= verticalAxis && (y >= horizontalAxis + 12 || y == horizontalAxis-10)) return Math.Abs(-x - y + 60) / Math.Sqrt(2);
            return Math.Abs(Math.Sqrt(x * x + (y + 10) * (y + 10)) - 50);
        }
	}
}
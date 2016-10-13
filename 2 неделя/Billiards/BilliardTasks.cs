using System;

namespace Billiards
{
	public class BilliardTasks
	{
		public double BounceVerticalWall(double directionRadians)
		{
            return Math.PI - directionRadians;
		}
		public double BounceHorizontalWall(double directionRadians)
		{
            return Math.PI*2 - directionRadians;
		}

		public double BounceWall(double directionRadians, double wallInclanationRadians)
		{
            return 2 * wallInclanationRadians - directionRadians;
		}

	}
}
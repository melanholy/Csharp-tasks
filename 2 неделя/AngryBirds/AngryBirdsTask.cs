using System;

namespace AngryBirds
{
	public class AngryBirdsTask
	{
        const double g = 9.8;
		public double FindSightAngle(double v, double distance)
		{
            return  Math.Asin(g*distance/(v*v))/2;
		}
	}
}

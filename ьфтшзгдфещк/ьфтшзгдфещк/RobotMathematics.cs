using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manipulator
{
	public static class RobotMathematics
	{
		const double UpperArm = 150;
		const double Forearm = 120;
		const double Palm = 100;

		public static double GetAngle(double c, double a, double b)
		{
            var hypotenuse = Math.Max(a, Math.Max(b, c));
            if (a == b && b == c) return Math.PI / 3;
            if (a == hypotenuse) return Math.Acos(b / a);
            if (b == hypotenuse) return Math.Acos(a / b);
            if (c == hypotenuse) return Math.PI - Math.Acos(b / c) - Math.Acos(a / c);
			throw new NotImplementedException();
		}

		public static double[] MoveTo(double x, double y, double angle)
		{
            var angles = new double[3];
            var x1 = x - Palm * Math.Cos(angle);
            var y1 = y + Palm * Math.Sin(angle);
            var dist = Math.Sqrt((50 - x1) * (50 - x1) + (400 - y1) * (400 - y1));
            var alpha = Math.Acos((UpperArm*UpperArm - Forearm*Forearm - dist * dist) / (-2*Forearm * dist));
            var gamma = Math.Acos((Forearm * Forearm - UpperArm * UpperArm - dist * dist) / (-2 * UpperArm * dist));
            var da = Math.Sqrt((x - 50) * (x - 50) + (y - 400) * (y - 400));
            var betta = Math.Acos((da * da - dist * dist - Palm * Palm) / (-2 * dist * Palm));
            angles[2] = alpha + betta;
            angles[1] = Math.PI - gamma - alpha;
            angles[0] = gamma + Math.PI / 2 - Math.Acos((400 - y1) / dist);
            return angles;
		}
	}
}

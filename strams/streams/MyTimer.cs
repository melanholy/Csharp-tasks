using System.Diagnostics;

namespace streams
{
	public class MyTimer : Stopwatch
	{
		public new TimerToken Start()
		{
			base.Start();
			return new TimerToken(this);
		}

		public TimerToken Continue()
		{
			base.Start();
			return new TimerToken(this);
		}
	}
}

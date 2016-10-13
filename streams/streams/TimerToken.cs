using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace streams
{
    public class TimerToken : IDisposable
    {
        private readonly Stopwatch Timer;

        public TimerToken(Stopwatch timer)
        {
            Timer = timer;
        }

        public void Dispose()
        {
            Timer.Stop();
        }
    }
}

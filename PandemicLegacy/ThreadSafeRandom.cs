using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PandemicLegacy
{
    public static class ThreadSafeRandom
    {
        [ThreadStatic]
        private static Random _random;
        public static Random ThisThreadsRandom
        {
            get
            {
                return _random ?? (_random = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId)));
            }
        }
    }
}

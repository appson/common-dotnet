using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appson.Common.Threading
{
    public class Throttler
    {
        public Throttler(int delayMs)
        {
            DelayMs = delayMs;
        }

        public int DelayMs { get; private set; }
    }
}

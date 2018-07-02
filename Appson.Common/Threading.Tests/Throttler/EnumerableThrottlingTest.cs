using System;
using System.Linq;
using Appson.Common.Threading.Extensions;
using static System.Console;

namespace Appson.Common.Threading.Tests.Throttler
{
    public class EnumerableThrottlingTest
    {
        public void Run()
        {
            var options = new ThrottlerOptions
            {
                DelayMillis = 20,
                RatePerSecond = 1000,
                InitialQuota = 0,
                MaxBurstSize = 100
            };

            var throttler = new ThrottlerThread(options);
            throttler.Start();

            var second = -1;
            var count = 0;

            var lesEnumerables = Enumerable.Range(0, 10000);
            var throtteledEnumerables = lesEnumerables.Throttle(throttler);

            foreach (var i in throtteledEnumerables)
            {
                if (second != DateTime.Now.Second)
                {
                    second = DateTime.Now.Second;
                    WriteLine($" - {count} times");
                    Write($"{second:D2}: ");
                    count = 0;
                }
                count++;
            }
            throttler.Stop();
            WriteLine($"done!");
        }
    }
}
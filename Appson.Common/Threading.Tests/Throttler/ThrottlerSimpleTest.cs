using System;

using static System.Console;

namespace Appson.Common.Threading.Tests.Throttler
{
    public class ThrottlerSimpleTest
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

            while (true)
            {
                if (second != DateTime.Now.Second)
                {
                    second = DateTime.Now.Second;
                    WriteLine($" - {count} times");
                    Write($"{second:D2}: ");
                    count = 0;
                }

                throttler.Throttle();
                if (count % 20 == 0)
                    Write(".");
                count++;
            }
        }
    }
}
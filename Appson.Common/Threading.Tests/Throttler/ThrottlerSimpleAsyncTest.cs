using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace Appson.Common.Threading.Tests.Throttler
{
    public class ThrottlerSimpleAsyncTest
    {
        public void Run()
        {
            var options = new ThrottlerOptions
            {
                DelayMillis = 20,
                RatePerSecond = 500,
                InitialQuota = 0,
                MaxBurstSize = 100
            };

            var throttler = new ThrottlerThread(options);
            var count = 0;

            WriteLine("Schduling 1 million tasks...");

            for (int i = 0; i < 1000000; i++)
            {
                Task.Run(async () =>
                {
                    await throttler.ThrottleAsync();
                    var totalCount = Interlocked.Increment(ref count);

                    if (totalCount % 10 == 0)
                        Write(".");
                });
            }

            throttler.Start();
            WriteLine("Throttler started.");

            while (true)
            {
                var sw = new Stopwatch();
                sw.Start();
                ReadLine();
                sw.Stop();

                var output = "";
                output += Environment.NewLine;
                output += Environment.NewLine;
                output += $"Elapsed: {sw.Elapsed}, Total: {count}, Rate: {(double)count / sw.Elapsed.TotalMilliseconds * 1000} items/s";
                output += Environment.NewLine;
                output += Environment.NewLine;

                WriteLine(output);
                count = 0;
            }

        }
    }
}
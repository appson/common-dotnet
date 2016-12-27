using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using static System.Console;

namespace Appson.Common.Threading.Tests.Throttler
{
    public class ThrottlerMultiThreadedTest
    {
        public void Run()
        {
            var options = new ThrottlerOptions
            {
                DelayMillis = 10,
                RatePerSecond = 2000,
                InitialQuota = 0,
                MaxBurstSize = 500
            };

            var throttler = new ThrottlerThread(options);
            throttler.Start();

            var count = 0;
            var threadIds = "12345";
//            var threadIds = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var counts = new int[threadIds.Length];

            for (var i = 0; i < threadIds.Length; i++)
            {
                var thread = new Thread(o =>
                {
                    var ii = (int)o;
                    var id = threadIds[ii];

                    while (true)
                    {
                        throttler.Throttle();

                        var myCount = Interlocked.Increment(ref count);
                        Interlocked.Increment(ref counts[ii]);

//                        var myCount = counts[ii];
                        if (myCount % 20 == 0)
                            Write(id);
                    }
                });

                thread.Start(i);
            }

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
                output += string.Join(",\t", counts.Select(c => c.ToString("D5")));
                output += Environment.NewLine;
                output += Environment.NewLine;

                WriteLine(output);
                count = 0;
                counts = new int[threadIds.Length];
            }
        }
    }
}
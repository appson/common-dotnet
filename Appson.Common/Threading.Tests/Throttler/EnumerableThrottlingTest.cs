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
            WriteLine("Specify the release count per second:");
            var rps = ReadLine();

            int.TryParse(rps, out int parsedRps);

            var second = -1;
            var count = 0;

            var lesEnumerables = Enumerable.Range(0, 1000000).ToList();
            var throttledEnumerables = lesEnumerables.Throttle(parsedRps);

            foreach (var _ in throttledEnumerables)
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
            WriteLine("done (throttled)");
        }
    }
}
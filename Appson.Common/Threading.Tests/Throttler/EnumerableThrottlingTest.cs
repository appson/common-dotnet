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
            var second = -1;
            var count = 0;

            var lesEnumerables = Enumerable.Range(0, 1000000).ToList();
            var throttledEnumerables = lesEnumerables.Throttle(50);

            foreach (var item in lesEnumerables)
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

            foreach (var item in throttledEnumerables)
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
        }
    }
}
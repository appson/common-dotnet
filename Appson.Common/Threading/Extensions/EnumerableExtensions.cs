using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Appson.Common.Threading.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Throttle<T>(this IEnumerable<T> self, int releaseCount)
        {
            if (releaseCount == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(releaseCount), releaseCount, @"Invalid value provided");
            }

            var signaler = new ManualResetEventSlim();

            var sleepDuration = Stopwatch.Frequency / releaseCount;

            foreach (var item in self)
            {
                Nop(sleepDuration);
                yield return item;
            }
        }

        private static void Nop(long duration)
        {
            var handle = Stopwatch.StartNew();
            while (handle.ElapsedTicks < duration)
            {
                // busy wait! :|
            }
        }
    }
}
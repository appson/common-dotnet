using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Appson.Common.Threading.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable Throttle(this IEnumerable self, int releaseCount)
        {
            if (releaseCount == 0)
                throw new ArgumentOutOfRangeException(nameof(releaseCount), releaseCount, @"Invalid value provided");

            var sleepDuration = 1000 / releaseCount;
            foreach (var item in self)
            {
                Thread.Sleep(sleepDuration);
                yield return item;
            }
        }

        public static IEnumerable<T> Throttle<T>(this IEnumerable<T> self, int releaseCount)
        {
            if (releaseCount == 0)
                throw new ArgumentOutOfRangeException(nameof(releaseCount), releaseCount, @"Invalid value provided");

            var sleepDuration = 1000 / releaseCount;
            foreach (var item in self)
            {
                Thread.Sleep(sleepDuration);
                yield return item;
            }
        }
    }
}
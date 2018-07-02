using System;
using System.Collections;
using System.Collections.Generic;

namespace Appson.Common.Threading.Extensions
{
    public static class ThrottlerThreadExtensions
    {
        public static IEnumerable Throttle(this IEnumerable self, ThrottlerThread throttler)
        {
            if (throttler == null)
                throw new ArgumentException(@"Exception occured while throttling because the throttler object was null",
                    nameof(throttler));
            foreach (var item in self)
            {
                throttler.Throttle();
                yield return item;
            }
        }

        public static IEnumerable<T> Throttle<T>(this IEnumerable<T> self, ThrottlerThread throttler)
        {
            if (throttler == null)
                throw new ArgumentException(@"Exception occured while throttling because the throttler object was null",
                    nameof(throttler));
            foreach (var item in self)
            {
                throttler.Throttle();
                yield return item;
            }
        }
    }
}
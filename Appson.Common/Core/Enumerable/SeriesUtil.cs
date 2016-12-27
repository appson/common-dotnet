using System.Collections.Generic;

namespace Appson.Common.Core.Enumerable
{
    public static class SeriesUtil
    {
        /// <summary>
        /// Generates Fibonacci series as an infinite Enumerable
        /// </summary>
        public static IEnumerable<long> Fibonacci()
        {
            long prev = 0;
            long current = 1;

            yield return 0;
            yield return 1;
            while (true)
            {
                var next = prev + current;
                prev = current;
                current = next;
                yield return current;
            }

            // ReSharper disable once IteratorNeverReturns
        }

        public static IEnumerable<long> NaturalNumbers()
        {
            var i = 1;
            while (true)
                yield return i++;

            // ReSharper disable once IteratorNeverReturns
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using Appson.Common.Core.Enumerable;

namespace Appson.Common.Threading.Tests.Throttler
{
    public class ThrottlerTplDataflowAsyncTest
    {
        public void Run()
        {
            var options = new ThrottlerOptions
            {
                DelayMillis = 20,
                RatePerSecond = 20,
                InitialQuota = 0,
                MaxBurstSize = 100
            };

            var throttler = new ThrottlerThread(options);
            throttler.Start();

            // To control the number of scheduled tasks / items posted in the TPL Dataflow
            SemaphoreSlim semaphore = new SemaphoreSlim(5000);

            var txBlock = new TransformBlock<long, long>(
                async i =>
                {
                    await throttler.ThrottleAsync();
                    Console.Write($"{i} ");
                    return i;
                }, new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 10
                });

            var actionBlock = new ActionBlock<long>(i => semaphore.Release());
            txBlock.LinkTo(actionBlock);

            foreach (var i in SeriesUtil.NaturalNumbers())
            {
                semaphore.Wait();
                txBlock.Post(i);
                if (i % 20 == 0)
                    Console.WriteLine($"\n\n{i}\n\n");
            }
        }

    }
}
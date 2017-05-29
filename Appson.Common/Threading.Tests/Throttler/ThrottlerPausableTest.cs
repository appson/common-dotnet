using System;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace Appson.Common.Threading.Tests.Throttler
{
    public class ThrottlerPausableTest
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

            var timer = new System.Timers.Timer(5000);
            timer.Elapsed += (sender, args) =>
            {
                Write($"{Environment.NewLine}changing throttler state: ");
                ChangeThrottlerState(throttler);
                WriteLine($"throttler is now {throttler.State.ToString()}");
            };
            timer.Enabled = true;
            timer.Start();

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
                count++;
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private static void ChangeThrottlerState(ThrottlerThread throttler)
        {
            if (throttler == null)
                return;
            switch (throttler.State)
            {
                case ThrottlerState.Started:
                    throttler.Pause();
                    break;
                case ThrottlerState.Stopped:
                    break;
                case ThrottlerState.Paused:
                    throttler.Resume();
                    break;
                case ThrottlerState.Unkown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
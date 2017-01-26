using System;
using System.Threading;
using System.Threading.Tasks;

namespace Appson.Common.Threading
{
    public class ThrottlerThread
    {
        private readonly Thread _thread;
        private readonly ThrottlerOptions _options;
        private readonly SemaphoreSlim _semaphore;

        private bool _started;
        private bool _stopped;
        private long _ticksPerRelease;
        private long _nextReleaseSchedule;

        public ThrottlerThread(ThrottlerOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options = options;
            ValidateOptions();
            CalculateRate();

            _thread = new Thread(ThreadStart);
            _semaphore = new SemaphoreSlim(_options.InitialQuota);
            _started = false;
            _stopped = false;
        }

        public void Start()
        {
            if (_started)
                throw new InvalidOperationException("The thread is already started.");

            _started = true;
            _thread.Start();
        }

        public void Stop(bool waitTillStopped = true)
        {
            if (!_started || _stopped)
                return;

            _stopped = true;

            if (waitTillStopped)
                _thread.Join();
        }

        public void Throttle()
        {
            _semaphore.Wait();
        }

        public async Task ThrottleAsync()
        {
            await _semaphore.WaitAsync();
        }

        private void ValidateOptions()
        {
            if (_options.DelayMillis < 1)
                throw new ArgumentException($"{nameof(_options.DelayMillis)} should be a positive integer.");

            if (_options.RatePerSecond <= 0)
                throw new ArgumentException($"{nameof(_options.RatePerSecond)} cannot be negative or zero.");

            if (_options.RatePerSecond > 500000d) // Larger than 500k per second
                throw new ArgumentException($"{nameof(_options.RatePerSecond)} larger than 500k/s is not supported because of %2.5+ inherent error in calculations.");

            if (_options.MaxBurstSize < 0)
                throw new ArgumentException($"{nameof(_options.MaxBurstSize)} cannot be a negative number.");

            if (_options.InitialQuota < 0)
                throw new ArgumentException($"{nameof(_options.InitialQuota)} cannot be a negative number.");

            if (_options.InitialQuota > _options.MaxBurstSize)
                throw new ArgumentException($"{nameof(_options.InitialQuota)} cannot be larger than {nameof(_options.MaxBurstSize)}.");

            var maxAchievabeRate = 1000d/_options.DelayMillis*_options.MaxBurstSize;
            if (maxAchievabeRate < _options.RatePerSecond)
                throw new ArgumentException($"The specified {nameof(_options.RatePerSecond)} is not achievable given the specified {nameof(_options.MaxBurstSize)} and {nameof(_options.DelayMillis)}. " +
                                            $"Because when a maximum of {_options.MaxBurstSize} items can be released in each iteration that takes at least {_options.DelayMillis}ms, the maximum " +
                                            $"achievable rate would be {maxAchievabeRate} that is lower than the specified rate of {_options.RatePerSecond} releases per second.");
        }

        private void CalculateRate()
        {
            _ticksPerRelease = (long) Math.Round(10000000d/_options.RatePerSecond);
        }

        private void ThreadStart()
        {
            _nextReleaseSchedule = DateTime.UtcNow.Ticks + _ticksPerRelease;
            while (!_stopped)
            {
                Thread.Sleep(_options.DelayMillis);
                
                var now = DateTime.UtcNow.Ticks;
                if (_nextReleaseSchedule > now)
                    continue; // The time for the next release is not yet reached

                var releaseCount = (int) ((now - _nextReleaseSchedule)/_ticksPerRelease + 1);
                var currentCount = _semaphore.CurrentCount;

                // Semaphore's CurrentCount shouldn't reach a number larger than MaxBurstSize after release
                releaseCount = Math.Min(releaseCount, _options.MaxBurstSize - currentCount);

                // Note: concurrency may result in the "CurrentCount" value to change after reading it and before releasing.
                // Since the only thread releasing the semaphore is this, the concurrent change is always zero or negative
                // (caused by another thread "wait"ing on the semaphore). So, the semaphore might not be released enough number
                // of times, but it will be compensated for in the next iteration of the loop.
                // Basically, we are "assuming" (and not enforcing) a serialized execution model where all "wait"s after the
                // delay is considered to be after the complete loop execution.

                if (releaseCount <= 0)
                    continue;

                _semaphore.Release(releaseCount);
                _nextReleaseSchedule += releaseCount*_ticksPerRelease;
            }
        }
    }
}

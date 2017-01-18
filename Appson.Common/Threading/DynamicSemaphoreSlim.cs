using System;
using System.Threading;
using System.Threading.Tasks;

namespace Appson.Common.Threading
{
    internal class DynamicSemaphoreSlim : IDisposable
    {
        private readonly object _lockObject;
        private readonly SemaphoreSlim _innerSemaphore;
        private int _maximum;

        public DynamicSemaphoreSlim(int initialCount)
            : this(initialCount, initialCount)
        {
        }

        public DynamicSemaphoreSlim(int initialCount, int maximum)
        {
            if (initialCount < 0)
                throw new ArgumentOutOfRangeException(nameof(initialCount));

            if (maximum < initialCount)
                throw new ArgumentOutOfRangeException(nameof(maximum));

            _lockObject = new object();
            _innerSemaphore = new SemaphoreSlim(initialCount);
            _maximum = maximum;
        }

        public void Dispose()
        {
            _innerSemaphore.Dispose();
        }

        public void SetMaximum(int newMaximum)
        {
            
        }

        public void Wait()
        {
            _innerSemaphore.Wait();
        }

        public Task WaitAsync()
        {
            return _innerSemaphore.WaitAsync();
        }

        public int Release()
        {
            lock (_lockObject)
            {
                // Check maximum change
            }
            return _innerSemaphore.Release();
        }

        public int CurrentCount => _innerSemaphore.CurrentCount;
    }
}
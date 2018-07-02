using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appson.Composer;
using Appson.Composer.Cache;

namespace Appson.Common.GeneralComponents.Cache.Components
{
    [Component]
    [ComponentCache(typeof(DefaultComponentCache))]
    [IgnoredOnAssemblyRegistration]
    public class AsyncAutoLoadItemCache<TKey, TValue> : AutoLoadItemCacheBase<TKey, TValue>, IAsyncItemCache<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, Task<CacheItem<TValue>>> _cacheData;

        [ComponentPlug]
        public IAsyncCacheItemLoader<TKey, TValue> ItemLoader { get; set; }

        public AsyncAutoLoadItemCache()
        {
            _cacheData = new ConcurrentDictionary<TKey, Task<CacheItem<TValue>>>();
        }

        #region Implementation of IAsyncCache<in TKey,out TValue>

        public void InvalidateAll()
        {
            _cacheData.Clear();
        }

        public async Task<TValue> GetItem(TKey key)
        {
                var creationTimeLimit = DateTime.UtcNow.Ticks - MaximumLifetimeSeconds * TimeSpan.TicksPerSecond;

                var item = await _cacheData.GetOrAdd(key, async k => new CacheItem<TValue>(await ItemLoader.Load(k)));
                if (item.CreationTime < creationTimeLimit)
                {
                    Task<CacheItem<TValue>> removedValue;
                    _cacheData.TryRemove(key, out removedValue);

                    item = await _cacheData.GetOrAdd(key, async k => new CacheItem<TValue>(await ItemLoader.Load(k)));
                }

                var result = item.Value;

                CheckForMaintenance();
                return Copier.Copy(result);
        }
        #endregion

        #region Implementation of IItemCache<in TKey,out TValue>

        public void InvalidateItem(TKey key)
        {
            Task<CacheItem<TValue>> item;

            if (_cacheData.ContainsKey(key))
                _cacheData.TryRemove(key, out item);
        }

        public void InvalidateItems(IEnumerable<TKey> keys)
        {
            foreach (var key in keys)
            {
                InvalidateItem(key);
            }
        }

        #endregion

        private void CheckForMaintenance()
        {
            if ((MaintenanceFrequencySeconds <= 0) ||
                (_cacheData.Count < MinimumSize) ||
                (_lastMaintenance + MaintenanceFrequencySeconds * TimeSpan.TicksPerSecond > DateTime.UtcNow.Ticks))
                return;

            PerformMaintenance();
        }

        private void PerformMaintenance()
        {
            _lastMaintenance = DateTime.UtcNow.Ticks;

            var creationTimeLimit = DateTime.UtcNow.Ticks - MinimumLifetimeSeconds * TimeSpan.TicksPerSecond;
            var lastAccessLimit = DateTime.UtcNow.Ticks - IdleSecondsToRemove * TimeSpan.TicksPerSecond;

            var keysToRemove = _cacheData.Where(itemTask =>
            {
                var item = itemTask.Value.Result;
                return item.CreationTime < creationTimeLimit &&
                           item.LastAccessTime < lastAccessLimit;
            }).Select(item => item.Key).ToList();

            Task<CacheItem<TValue>> removedValue;
            foreach (var key in keysToRemove)
                _cacheData.TryRemove(key, out removedValue);
        }
    }
}
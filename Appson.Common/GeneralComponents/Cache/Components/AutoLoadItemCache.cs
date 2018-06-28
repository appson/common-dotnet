using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Appson.Composer;
using Appson.Composer.Cache;

namespace Appson.Common.GeneralComponents.Cache.Components
{
    [Component]
    [ComponentCache(typeof(DefaultComponentCache))]
    [IgnoredOnAssemblyRegistration]
    public class AutoLoadItemCache<TKey, TValue> : AutoLoadItemCacheBase<TKey, TValue>, IItemCache<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, CacheItem<TValue>> _cacheData;

        [ComponentPlug]
        public ICacheItemLoader<TKey, TValue> ItemLoader { get; set; }

        protected AutoLoadItemCache()
        {
            _cacheData = new ConcurrentDictionary<TKey, CacheItem<TValue>>();
        }

        #region Implementation of ICache<in TKey,out TValue>

        public void InvalidateAll()
        {
            _cacheData.Clear();
        }

        public TValue this[TKey key]
        {
            get
            {
                var creationTimeLimit = DateTime.UtcNow.Ticks - MaximumLifetimeSeconds * TimeSpan.TicksPerSecond;

                var item = _cacheData.GetOrAdd(key, k => new CacheItem<TValue>(ItemLoader.Load(k)));
                if (item.CreationTime < creationTimeLimit)
                {
                    CacheItem<TValue> removedValue;
                    _cacheData.TryRemove(key, out removedValue);

                    item = _cacheData.GetOrAdd(key, k => new CacheItem<TValue>(ItemLoader.Load(k)));
                }

                var result = item.Value;


                CheckForMaintenance();
                return Copier.Copy(result);
            }
        }
        #endregion

        #region Implementation of IItemCache<in TKey,out TValue>

        public void InvalidateItem(TKey key)
        {
            CacheItem<TValue> item;

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

            var keysToRemove = _cacheData.Where(item => item.Value.CreationTime < creationTimeLimit && item.Value.LastAccessTime < lastAccessLimit).Select(item => item.Key).ToList();

            CacheItem<TValue> removedValue;
            foreach (var key in keysToRemove)
                _cacheData.TryRemove(key, out removedValue);
        }
    }
}
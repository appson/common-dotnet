using System;
using Appson.Common.GeneralComponents.Cache;
using Appson.Common.GeneralComponents.Cache.Components;
using Appson.Composer;
using Appson.Composer.Cache;

[Component]
[ComponentCache(typeof(DefaultComponentCache))]
[IgnoredOnAssemblyRegistration]
public class AutoLoadItemCache<TKey, TValue> : AutoLoadItemCacheBase<TKey, TValue>, IItemCache<TKey, TValue>
{
    [ComponentPlug]
    public ICacheItemLoader<TKey, TValue> ItemLoader { get; set; }

    public TValue this[TKey key]
    {
        get
        {
            var creationTimeLimit = DateTime.UtcNow.Ticks - MaximumLifetimeSeconds * 10000000;

            var item = _cacheData.GetOrAdd(key, k => new CacheItem<TValue>(ItemLoader.Load(k)));
            if (item.LastAccessTime < creationTimeLimit)
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
}
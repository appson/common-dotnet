using System.Collections.Generic;
using Appson.Common.Web.Owin.RequestScopeContext;
using Appson.Composer;
using Appson.Composer.Cache;

namespace Appson.Common.Web.Owin.Composer
{
    [Contract]
    [ComponentCache(typeof(StaticComponentCache))]
    [Component]
    public class OwinContextComponentCache : IComponentCache
    {
        private const string ContextKey = "Appson.Composer.Cache.Owin";
        private readonly object _nullRequestSyncObject = new object();

        public static bool CanBeUsed => OwinRequestScopeContext.Current != null;

        public object SynchronizationObject
        {
            get
            {
                var context = OwinRequestScopeContext.Current;
                return context != null ? context.SyncObject : _nullRequestSyncObject;
            }
        }

        public ComponentCacheEntry GetFromCache(ContractIdentity contract)
        {
            var context = OwinRequestScopeContext.Current;

            if (context == null)
                return null;

            var cache = GetCacheDictionary(context);

            if (cache.ContainsKey(contract))
                return cache[contract];

            return null;
        }

        public void PutInCache(ContractIdentity contract, ComponentCacheEntry entry)
        {
            GetCacheDictionary(OwinRequestScopeContext.Current)[contract] = entry;
        }

        private Dictionary<ContractIdentity, ComponentCacheEntry> GetCacheDictionary(OwinRequestScopeContext context)
        {
            return context.Items.GetOrAdd(ContextKey, s => new Dictionary<ContractIdentity, ComponentCacheEntry>())
                as Dictionary<ContractIdentity, ComponentCacheEntry>;
        }
    }
}
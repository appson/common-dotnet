using System.Collections.Generic;
using System.Threading.Tasks;
using Appson.Composer;

namespace Appson.Common.GeneralComponents.Cache
{
    [Contract]
    public interface IAsyncCache<in TKey, TValue>
    {
        void InvalidateAll();
        Task<TValue> GetItem(TKey key);
    }

    [Contract]
    public interface IAsyncItemCache<in TKey, TValue>: IAsyncCache<TKey, TValue>
    {
        void InvalidateItem(TKey key);
        void InvalidateItems(IEnumerable<TKey> keys);
    }
}
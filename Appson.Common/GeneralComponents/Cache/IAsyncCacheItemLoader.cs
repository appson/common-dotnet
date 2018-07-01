using System.Threading.Tasks;

namespace Appson.Common.GeneralComponents.Cache.Components
{
    public interface IAsyncCacheItemLoader<in TKey, TValue>
    {
        Task<TValue> Load(TKey key);
    }
}
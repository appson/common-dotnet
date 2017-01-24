
using Appson.Composer;

namespace Appson.Common.GeneralComponents.Cache
{
    [Contract]
	public interface IWritableCache<in TKey, TValue> : ICache<TKey, TValue>
	{
		TValue Put(TKey key, TValue value);
	}
}
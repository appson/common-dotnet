using Compositional.Composer;

namespace Appson.Common.Cache
{
	[Contract]
	public interface IWritableCache<in TKey, TValue> : ICache<TKey, TValue>
	{
		TValue Put(TKey key, TValue value);
	}
}
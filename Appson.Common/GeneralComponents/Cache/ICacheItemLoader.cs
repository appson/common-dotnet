using Compositional.Composer;

namespace Appson.Common.Cache
{
	[Contract]
	public interface ICacheItemLoader<in TKey, out TValue>
	{
		TValue Load(TKey key);
	}
}
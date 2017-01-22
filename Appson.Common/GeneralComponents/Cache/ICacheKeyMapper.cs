using Compositional.Composer;

namespace Appson.Common.Cache
{
	[Contract]
	public interface ICacheKeyMapper<out TKey, in TValue>
	{
		TKey MapKey(TValue value);
	}
}
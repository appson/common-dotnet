
using Appson.Composer;

namespace Appson.Common.GeneralComponents.Cache
{
    [Contract]
	public interface ICacheItemLoader<in TKey, out TValue>
	{
		TValue Load(TKey key);
	}
}

using Appson.Composer;

namespace Appson.Common.GeneralComponents.Cache
{
    [Contract]
	public interface ICacheKeyMapper<out TKey, in TValue>
	{
		TKey MapKey(TValue value);
	}
}
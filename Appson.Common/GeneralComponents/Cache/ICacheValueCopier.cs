
using Appson.Composer;

namespace Appson.Common.GeneralComponents.Cache
{
    [Contract]
	public interface ICacheValueCopier<TValue>
	{
		TValue Copy(TValue original);
	}
}
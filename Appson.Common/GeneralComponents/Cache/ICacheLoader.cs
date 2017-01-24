using System.Collections.Generic;
using Appson.Composer;

namespace Appson.Common.GeneralComponents.Cache
{
    [Contract]
	public interface ICacheLoader<out TValue>
	{
		IEnumerable<TValue> LoadAll();
	}
}
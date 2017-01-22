using System.Collections.Generic;
using Compositional.Composer;

namespace Appson.Common.Cache
{
	[Contract]
	public interface ICacheLoader<out TValue>
	{
		IEnumerable<TValue> LoadAll();
	}
}
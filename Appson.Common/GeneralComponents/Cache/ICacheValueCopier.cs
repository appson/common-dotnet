using Compositional.Composer;

namespace Appson.Common.Cache
{
	[Contract]
	public interface ICacheValueCopier<TValue>
	{
		TValue Copy(TValue original);
	}
}
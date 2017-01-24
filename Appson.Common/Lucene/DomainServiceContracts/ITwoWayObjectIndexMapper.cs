using Appson.Composer;
using Compositional.Composer;
using Lucene.Net.Documents;

namespace Appson.Common.DomainServiceContracts
{
	[Contract]
	public interface ITwoWayObjectIndexMapper<in TInput, out TOutput> : IObjectIndexMapper<TInput>
	{
		TOutput GetObject(Document document);
	}
}
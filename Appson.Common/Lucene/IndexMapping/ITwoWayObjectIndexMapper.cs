using Appson.Composer;
using Lucene.Net.Documents;

namespace Appson.Common.Lucene.IndexMapping
{
    [Contract]
	public interface ITwoWayObjectIndexMapper<in TInput, out TOutput> : IObjectIndexMapper<TInput>
	{
		TOutput GetObject(Document document);
	}
}
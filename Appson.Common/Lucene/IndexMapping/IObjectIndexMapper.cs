using Appson.Composer;
using Lucene.Net.Documents;

namespace Appson.Common.Lucene.IndexMapping
{
    [Contract]
	public interface IObjectIndexMapper<in T>
	{
		void PopulateDocument(T obj);
		Document GetDocument();

		bool DocumentReady { get; }
	}
}

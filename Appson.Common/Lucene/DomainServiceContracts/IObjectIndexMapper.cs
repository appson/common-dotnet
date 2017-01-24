using Appson.Composer;
using Compositional.Composer;
using Lucene.Net.Documents;

namespace Appson.Common.DomainServiceContracts
{
	[Contract]
	public interface IObjectIndexMapper<in T>
	{
		void PopulateDocument(T obj);
		Document GetDocument();

		bool DocumentReady { get; }
	}
}

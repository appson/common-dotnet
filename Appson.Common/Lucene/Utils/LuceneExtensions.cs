using System.Collections.Generic;
using Lucene.Net.Search;

namespace Appson.Common.Lucene.Utils
{
	public static class LuceneExtensions
	{
		public static void AddAll(this BooleanQuery baseQuery, IEnumerable<Query> queries, Occur occur)
		{
			queries.ForEach(q => baseQuery.Add(q, occur));
		}
	}
}
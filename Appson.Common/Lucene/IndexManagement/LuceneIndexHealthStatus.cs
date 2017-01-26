using System;

namespace Appson.Common.Lucene.IndexManagement
{
    public class LuceneIndexHealthStatus
	{
		public string IndexID { get; set; }
		public bool Initialized { get; set; }
		public bool ShutDown { get; set; }

		public int NumberOfErrors { get; set; }
		public DateTime? IndexSearcherLastUsedTimeUtc { get; set; }
		public DateTime? IndexWriterLastUsedTimeUtc { get; set; }
		public DateTime? LastCommitTimeUtc { get; set; }
		public DateTime? LastOptimizationTimeUtc { get; set; }

		public bool HasErrors => NumberOfErrors > 0;
	}
}
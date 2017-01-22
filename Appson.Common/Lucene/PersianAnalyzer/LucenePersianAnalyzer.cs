using log4net;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;

namespace Appson.Common.Lucene.PersianAnalyzer
{
    public class LucenePersianAnalyzer : StandardAnalyzer
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(LucenePersianAnalyzer));

		public LucenePersianAnalyzer()
			: base(Version.LUCENE_30)
		{
			// TODO: Customize this to do Persian-specific analysis, stemming, stop-word removal, and normalization
		}
	}
}
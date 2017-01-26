using Appson.Common.General.Text;
using log4net;

namespace Appson.Common.Lucene.Utils
{
	public static class UserSearchQueryUtils
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(UserSearchQueryUtils));

		public static string[] TokenizeUserQuery(string query)
		{
			return query.SplitByWhitespace();
		}
	}
}
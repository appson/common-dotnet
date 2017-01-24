using System.Linq;
using log4net;

namespace Appson.Common.EntityFramework
{
	public static class QueryExtensions
	{
        private static readonly ILog SqlTraceLog = LogManager.GetLogger("Appson.Common.EntityFramework.SqlTrace");

        public static IQueryable<T> TraceSql<T>(this IQueryable<T> query)
		{
			var sql = query.ToString();
            SqlTraceLog.Info(sql);

			return query;
		}
	}
}
using System.Data.Entity.Spatial;
using Appson.Common.General.Utils;
using Appson.Common.SqlGeo;
using ServiceStack.Text;

namespace Appson.Common.CodeSnippets.ServiceStack
{
	public static class JsSerializationConfigUtil
	{
		public static void InitializeDefaultConfiguration()
		{
			JsConfig.TreatEnumAsInteger = true;
			JsConfig.DateHandler = DateHandler.ISO8601;

			JsConfig<DbGeography>.SerializeFn = geography => geography.ToWkt();
			JsConfig<DbGeography>.DeSerializeFn = s => s.IfNotNull(DbGeography.FromText);
		}
	}
}
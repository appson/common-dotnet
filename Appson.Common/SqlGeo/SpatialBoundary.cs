using System.Data.Entity.Spatial;

namespace Appson.Common.SqlGeo
{
    public class SpatialBoundary
	{
		public DbGeography Geography { get; set; }
		public string Title { get; set; }
	}
}
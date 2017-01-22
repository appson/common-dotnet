using System.Data.Entity.Spatial;

namespace Appson.Common.Spatial
{
	public class SpatialBoundary
	{
		public DbGeography Geography { get; set; }
		public string Title { get; set; }
	}
}
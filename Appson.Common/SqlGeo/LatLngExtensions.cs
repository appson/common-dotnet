using System.Data.Entity.Spatial;
using Appson.Common.General.Geo;

namespace Appson.Common.SqlGeo
{
    public static class LatLngExtensions
    {
        public static DbGeography ToDbGeography(this LatLng latLng)
        {
            return DbGeographyUtil.CreatePoint(latLng.ToWkt());
        }

        public static DbGeography ToDbGeography(this LatLngBounds bounds)
        {
            return DbGeographyUtil.CreatePolygon(string.Format("POLYGON(({0} {1}, {0} {2}, {3} {2}, {3} {1}, {0} {1}))",
                bounds.EastLng, bounds.NorthLat, bounds.SouthLat, bounds.WestLng));
        }

        public static double GetArea(this LatLngBounds bounds)
        {
            return bounds.ToDbGeography().Area.GetValueOrDefault();
        }
    }
}
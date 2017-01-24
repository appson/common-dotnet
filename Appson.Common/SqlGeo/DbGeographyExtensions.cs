using System.Data.Entity.Spatial;
using Appson.Common.General.Geo;
using Appson.Common.General.Utils;

namespace Appson.Common.SqlGeo
{
    public static class DbGeographyExtensions
    {
        #region Extension methods

        public static string ToWkt(this DbGeography geography)
        {
            return geography.IfNotNull(g => g.AsText());
        }

        public static LatLng ToLatLng(this DbGeography geography)
        {
            if (geography == null || geography.IsEmpty || !geography.Latitude.HasValue || !geography.Longitude.HasValue)
                return null;

            return new LatLng { Lat = geography.Latitude.Value, Lng = geography.Longitude.Value };
        }

        public static LatLngPath ToLatLngPath(this DbGeography geography)
        {
            if (geography == null)
                return null;

            if (geography.IsEmpty || geography.StartPoint == null || !geography.PointCount.HasValue || geography.PointCount.Value < 1)
                return null;

            var result = new LatLngPath();
            for (var i = 1; i <= geography.PointCount.Value; i++)
                result.Points.Add(geography.PointAt(i).ToLatLng());

            return result;
        }

        #endregion
    }
}
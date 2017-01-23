using System;
using System.Data.Entity.Spatial;
using System.Data.SqlTypes;
using Appson.Common.General.Geo;
using Microsoft.SqlServer.Types;

namespace Appson.Common.SqlGeo
{
	public static class DbGeographyUtil
	{
        public static DbGeography CreatePoint(string wktString)
		{
			return string.IsNullOrWhiteSpace(wktString) ? null : DbGeography.FromText(wktString);
		}

		public static DbGeography CreatePolygon(string wktString)
		{
			if (string.IsNullOrWhiteSpace(wktString))
				return null;

			var sqlGeography = SqlGeography.STGeomFromText(new SqlChars(wktString), DbGeography.DefaultCoordinateSystemId).MakeValid();

			var invertedSqlGeography = sqlGeography.ReorientObject();
			if (sqlGeography.STArea() > invertedSqlGeography.STArea())
			{
				sqlGeography = invertedSqlGeography;
			}

			return DbSpatialServices.Default.GeographyFromProviderValue(sqlGeography);
		}

		public static LatLngBounds FindBoundingBox(this DbGeography geography)
		{
			if (geography == null)
				return null;

			if (geography.IsEmpty || geography.StartPoint == null || !geography.PointCount.HasValue || geography.PointCount.Value < 1)
				return null;
				
			double minLat = geography.StartPoint.Latitude.Value;
			double minLng = geography.StartPoint.Longitude.Value;
			double maxLat = geography.StartPoint.Latitude.Value;
			double maxLng = geography.StartPoint.Longitude.Value;

			if (geography.PointCount < 2)
				return new LatLngBounds {SouthWest = new LatLng {Lat = minLat, Lng = minLng}, NorthEast = new LatLng {Lat = maxLat, Lng = maxLng}};

			for (int i = 2; i <= geography.PointCount; i++)
			{
				var point = geography.PointAt(i);
				minLat = Math.Min(minLat, point.Latitude.Value);
				minLng = Math.Min(minLng, point.Longitude.Value);
				maxLat = Math.Max(maxLat, point.Latitude.Value);
				maxLng = Math.Max(maxLng, point.Longitude.Value);
			}

			return new LatLngBounds { SouthWest = new LatLng { Lat = minLat, Lng = minLng }, NorthEast = new LatLng { Lat = maxLat, Lng = maxLng } };
		}
	}
}
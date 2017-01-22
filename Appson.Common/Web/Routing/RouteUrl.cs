using System.Web.Routing;

namespace Appson.Common.Web.Routing
{
	/// <summary>
	/// Represents a generated route URL with route data.
	/// 
	/// Copied from:
	/// http://erraticdev.blogspot.de/2011/01/custom-aspnet-mvc-route-class-with.html
	/// 
	/// </summary>
	public class RouteUrl
	{
		/// <summary>
		/// Gets or sets the route URL.
		/// </summary>
		/// <value>Route URL.</value>
		public string Url { get; set; }


		/// <summary>
		/// Gets or sets route values.
		/// </summary>
		/// <value>Route values.</value>
		public RouteValueDictionary Values { get; set; }
	}
}
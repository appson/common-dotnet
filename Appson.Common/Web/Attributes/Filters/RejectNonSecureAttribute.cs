using System;
using System.Web.Mvc;

namespace Appson.Common.Web.Attributes.Filters
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class RejectNonSecureAttribute : RequireHttpsAttribute
	{
		protected override void HandleNonHttpsRequest(AuthorizationContext filterContext)
		{
			filterContext.Result = new HttpUnauthorizedResult("Only secure (HTTPS) connections are allowed.");
		}
	}
}
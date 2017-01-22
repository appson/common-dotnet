using System;
using System.Web.Mvc;
using Appson.Common.Configuration;

namespace Appson.Common.Web.Attributes.Filters
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class RejectNonSecureInProductionAttribute : RejectNonSecureAttribute
	{
		protected override void HandleNonHttpsRequest(AuthorizationContext filterContext)
		{
			if (ApplicationEnvironmentUtil.Type == ApplicationEnvironmentType.Development)
				return;

			base.HandleNonHttpsRequest(filterContext);
		}
	}
}
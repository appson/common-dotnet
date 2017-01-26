using System;
using System.Web.Mvc;
using Appson.Common.GeneralComponents.Configuration;
using Appson.Common.Web.Utils;

namespace Appson.Common.Web.Attributes.Filters
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
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
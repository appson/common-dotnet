
using System.Web.Http;
using Appson.Composer;
using Appson.Composer.WebApi.Resolver;

namespace Appson.Common.Web.Owin.Composer
{
    public class WebApiComposerWebUtil
	{
		public static void Setup(IComposer componentContext, HttpConfiguration configuration)
		{
			var webApiDependencyResolver = componentContext.GetComponent<IWebApiDependencyResolverContract>();
			if (webApiDependencyResolver == null)
				throw new CompositionException("No provider component for IWebApiDependencyResolverContract is registered. " +
				                               "Make sure you've included the correct libraries in the composition config.");

			configuration.DependencyResolver = webApiDependencyResolver;
		}
	}
}
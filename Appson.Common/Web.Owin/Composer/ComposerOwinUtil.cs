using System.Reflection;
using System.Web.Http;
using Appson.Common.Web.Owin.RequestScopeContext;
using Appson.Composer;
using Appson.Composer.Utility;

namespace Appson.Common.Web.Owin.Composer
{
    public static class ComposerOwinUtil
    {
        public static IComponentContext ComponentContext => OwinRequestScopeContext.Current.GetComponentContext();

        public static IComponentContext Setup()
        {
            var componentContext = new ComponentContext();
            Setup(componentContext);

            return componentContext;
        }

        public static void Setup(ComponentContext composer)
        {
            composer.RegisterAssembly(Assembly.GetExecutingAssembly());
            composer.ProcessApplicationConfiguration();
        }

        public static void SetResolver(IComponentContext componentContext, HttpConfiguration configuration)
        {
            var webApiDependencyResolver = componentContext.GetComponent<IWebApiDependencyResolverContract>();
            configuration.DependencyResolver = webApiDependencyResolver ?? throw new CompositionException(
                                                   "No provider component for IWebApiDependencyResolverContract is registered. " +
                                                   "Make sure you've included the correct libraries in the composition config.");
        }
    }
}
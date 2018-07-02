using System;
using System.Web.Http;
using Appson.Composer;

namespace Appson.Common.Web.Owin.Composer
{
    [Obsolete("Use ComposerOwinUtil instead")]
    public class WebApiComposerWebUtil
    {
        [Obsolete("Use ComposerOwinUtil instead")]
        public static void Setup(IComposer componentContext, HttpConfiguration configuration)
        {
            var webApiDependencyResolver = componentContext.GetComponent<IWebApiDependencyResolverContract>();
            configuration.DependencyResolver = webApiDependencyResolver ?? throw new CompositionException(
                                                   "No provider component for IWebApiDependencyResolverContract is registered. " +
                                                   "Make sure you've included the correct libraries in the composition config.");
        }
    }
}
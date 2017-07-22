using System.Web.Http;
using Appson.Common.Web.Owin.RequestScopeContext;
using Appson.Composer;
using Owin;

namespace Appson.Common.Web.Owin.Composer
{
    public static class AppBuilderComposerMiddlewareExtensions
    {
        public static IComponentContext UseRequestScopeContext(this IAppBuilder app, HttpConfiguration configuration = null)
        {
            var composer = ComposerOwinUtil.ComponentContext ??
                           ComposerOwinUtil.Setup(configuration ?? GlobalConfiguration.Configuration);

            app.Use(async (context, next) =>
            {
                context.SetComposer(composer);
                await next.Invoke();
            });

            return composer;
        }
    }
}
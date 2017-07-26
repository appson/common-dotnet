using System;
using System.Web.Http;
using Appson.Composer;
using Owin;

namespace Appson.Common.Web.Owin.Composer
{
    public static class AppBuilderComposerMiddlewareExtensions
    {
        public static IComponentContext UseComposer(this IAppBuilder app, HttpConfiguration configuration = null)
        {
            var composer = ComposerOwinUtil.Setup();
            app.UseComposer(composer, configuration);

            return composer;
        }

        public static void UseComposer(this IAppBuilder app, IComponentContext composer, HttpConfiguration configuration = null)
        {
            if (composer == null)
                throw new ArgumentNullException(nameof(composer));

            ComposerOwinUtil.SetResolver(composer, configuration ?? GlobalConfiguration.Configuration);

            app.Use(async (context, next) =>
            {
                context.SetComposer(composer);
                await next.Invoke();
            });
        }
    }
}
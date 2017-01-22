using Owin;

namespace Appson.Common.Owin
{
    public static class AppBuilderOwinRequestScopeContextMiddlewareExtensions
    {
        /// <summary>
        /// Use OwinRequestScopeContextMiddleware.
        /// </summary>
        /// <param name="app">Owin app.</param>
        /// <returns></returns>
        public static IAppBuilder UseRequestScopeContext(this IAppBuilder app)
        {
            return app.Use(typeof(OwinRequestScopeContextMiddleware));
        }
    }
}
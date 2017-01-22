using Compositional.Composer;
using JahanJooy.Common.Util.Owin;
using Microsoft.Owin;

namespace JahanJooy.Common.Util.Composer
{
    public static class ComposerOwinContextExtensions
    {
        private const string ComposerKey = "jj.Composer";
        private const string ComponentContextKey = "jj.ComponentContext";

        #region Retrieve methods

        public static IComposer GetComposer(this IOwinContext context)
        {
            return context.IfNotNull(c => c.Get<IComposer>(ComposerKey));
        }

        public static IComposer GetComposer(this OwinRequestScopeContext context)
        {
            return context.IfNotNull(c => c.OwinContext.GetComposer());
        }

        public static IComponentContext GetComponentContext(this IOwinContext context)
        {
            return context.IfNotNull(c => c.Get<IComponentContext>(ComponentContextKey));
        }

        public static IComponentContext GetComponentContext(this OwinRequestScopeContext context)
        {
            return context.IfNotNull(c => c.OwinContext.GetComponentContext());
        }

        #endregion

        #region Set methods

        public static void SetComposer(this IOwinContext context, IComposer composer)
        {
            context.Set(ComposerKey, composer);
        }

        public static void SetComposer(this OwinRequestScopeContext context, IComposer composer)
        {
            context.OwinContext.SetComposer(composer);
        }

        public static void SetComponentContext(this IOwinContext context, IComponentContext componentContext)
        {
            context.Set(ComponentContextKey, componentContext);
        }

        public static void SetComponentContext(this OwinRequestScopeContext context, IComponentContext componentContext)
        {
            context.OwinContext.SetComponentContext(componentContext);
        }

        #endregion

    }
}

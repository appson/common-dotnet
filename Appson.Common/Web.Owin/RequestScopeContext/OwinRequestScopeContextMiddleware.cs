using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Appson.Common.Web.Owin.RequestScopeContext
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class OwinRequestScopeContextMiddleware
    {
        private readonly AppFunc _next;
        private const string EnvironmentKey = "jj.OwinRequestScopeContext";

        public OwinRequestScopeContextMiddleware(AppFunc next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            if (environment.ContainsKey(EnvironmentKey))
            {
                // No need to create another scope, and no need to dispose it.
                // Just set it as current on the CallContext, and pass the request to the next module in pipeline.

                OwinRequestScopeContext.Current = (OwinRequestScopeContext) environment[EnvironmentKey];
                await _next(environment);
            }
            else
            {
                // This is the first time the module appears in the pipeline.
                // Create a new scope, set it in environment, and clean-up after the pipeline is complete.

                var scopeContext = new OwinRequestScopeContext(environment);
                environment[EnvironmentKey] = scopeContext;
                OwinRequestScopeContext.Current = scopeContext;

                try
                {
                    await _next(environment);
                }
                finally
                {
                    try
                    {
                        scopeContext.Complete();
                    }
                    finally
                    {
                        OwinRequestScopeContext.FreeContextSlot();
                    }
                }
            }
        }
    }
}
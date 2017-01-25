using System;
using Appson.Common.General.Text;
using Appson.Common.General.Utils;
using Appson.Common.Web.Owin.RequestScopeContext;
using Microsoft.Owin;

namespace Appson.Common.Web.Owin
{
    public static class CorrelationIdUtils
    {
        private const string OwinEnironmentKey = "Appson.CorrelationId";

        public static void EnsureCorrelationId(this IOwinContext context)
        {
            if (context.Get<object>(OwinEnironmentKey).SafeToString().IsNullOrWhitespace())
            {
                context.Set(OwinEnironmentKey, Guid.NewGuid().ToUrlFriendly());
            }
        }

        public static string GetCorrelationId(this IOwinContext context)
        {
            return context.Get<string>(OwinEnironmentKey);
        }

        public static string GetCorrelationId(this OwinRequestScopeContext context)
        {
            return context.OwinContext.Get<string>(OwinEnironmentKey);
        }

        public static string Current
        {
            get
            {
                return OwinRequestScopeContext.Current.IfNotNull(c => c.GetCorrelationId());
            }
        }
    }
}
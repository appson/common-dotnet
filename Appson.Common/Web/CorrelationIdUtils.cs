using System;
using System.Collections.Generic;
using Appson.Common.General;
using Appson.Common.Owin;
using Appson.Common.Text;
using Microsoft.Owin;

namespace Appson.Common.Web
{
    public static class CorrelationIdUtils
    {
        private const string OwinEnironmentKey = "jj.CorrelationId";

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
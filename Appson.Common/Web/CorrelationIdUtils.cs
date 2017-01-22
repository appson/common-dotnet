using System;
using System.Collections.Generic;
using JahanJooy.Common.Util.General;
using JahanJooy.Common.Util.Owin;
using JahanJooy.Common.Util.Text;
using Microsoft.Owin;

namespace JahanJooy.Common.Util.Web
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
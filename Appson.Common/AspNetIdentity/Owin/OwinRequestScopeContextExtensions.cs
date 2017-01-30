using System;
using System.Security.Principal;
using Appson.Common.General.Text;
using Appson.Common.General.Utils;
using Appson.Common.Web.Owin.RequestScopeContext;
using Microsoft.AspNet.Identity;

namespace Appson.Common.AspNetIdentity.Owin
{
    public static class OwinRequestScopeContextExtensions
    {
        public static IPrincipal GetUser(this OwinRequestScopeContext context)
        {
            return context.IfNotNull(c => c.OwinContext.IfNotNull(o => o.Authentication.IfNotNull(a => a.User)));
        }

        public static string GetUserId(this OwinRequestScopeContext context)
        {
            return context.GetUser().IfNotNull(u => u.Identity.IfNotNull(i => i.GetUserId()));
        }

        public static long? GetUserIdLong(this OwinRequestScopeContext context)
        {
            var userIdString = context.GetUserId();

            if (userIdString.IsNullOrWhitespace())
                return null;

            long result;
            if (long.TryParse(userIdString, out result))
                return result;

            return null;
        }

        public static Guid? GetUserIdGuid(this OwinRequestScopeContext context)
        {
            var userIdString = context.GetUserId();

            if (userIdString.IsNullOrWhitespace())
                return null;

            Guid result;
            if (Guid.TryParse(userIdString, out result))
                return result;

            return null;
        }

    }
}
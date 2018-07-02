using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Web.Http.Controllers;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Routing;
using Appson.Common.General.ExceptionUtils;
using Appson.Common.General.Utils;

namespace Appson.Common.Web.Owin.ExceptionHandling
{
    public static class ExceptionLoggerContextUtils
    {
        public static Dictionary<string, object> ToDetailsDictionary(this ExceptionLoggerContext context)
        {
            if (context == null)
                return null;

            var result = new Dictionary<string, object>();

            AddToDetailsObject(result, "Request", () => MapRequest(context.ExceptionContext.Request));
            AddToDetailsObject(result, "RequestContext", () => MapRequestContext(context.ExceptionContext.RequestContext));
            AddToDetailsObject(result, "ControllerContext", () => MapControllerContext(context.ExceptionContext.ControllerContext));
            AddToDetailsObject(result, "ActionContext", () => MapActionContext(context.ExceptionContext.ActionContext));
            AddToDetailsObject(result, "Response", () => MapResponse(context.ExceptionContext.Response));
            AddToDetailsObject(result, "StackTrace", () => context.Exception.StackTrace);
            AddToDetailsObject(result, "Exception", () => ExceptionDetailUtils.MapException(context.Exception, 0));

            return result;
        }

        private static object MapControllerContext(HttpControllerContext controllerContext)
        {
            if (controllerContext == null)
                return "<NULL>";

            return new Dictionary<string, string>
            {
                {"Name", controllerContext.ControllerDescriptor.IfNotNull(d => d.ControllerName)},
                {"Type", controllerContext.ControllerDescriptor.IfNotNull(d => d.ControllerType.FullName)}
            };
        }

        private static object MapActionContext(HttpActionContext actionContext)
        {
            if (actionContext == null)
                return "<NULL>";

            return new Dictionary<string, string>
            {
                {"Name", actionContext.ActionDescriptor.IfNotNull(d => d.ActionName)}
            };
        }

        private static object MapResponse(HttpResponseMessage response)
        {
            if (response == null)
                return "<NULL>";

            var result = new Dictionary<string, object>();

            AddToDetailsObject(result, "StatusCode", () => response.StatusCode);
            AddToDetailsObject(result, "Version", () => response.Version.SafeToString());
            AddToDetailsObject(result, "ReasonPhrase", () => response.ReasonPhrase);
            AddToDetailsObject(result, "IsSuccess", () => response.IsSuccessStatusCode);
            AddToDetailsObject(result, "Headers", () => MapHeaders(response.Headers));

            return result;
        }

        private static object MapRequestContext(HttpRequestContext requestContext)
        {
            if (requestContext == null)
                return "<NULL>";

            var result = new Dictionary<string, object>();

            AddToDetailsObject(result, "Principal", () => MapPrincipal(requestContext.Principal));
            AddToDetailsObject(result, "RouteData", () => MapRouteData(requestContext.RouteData));

            return result;
        }

        private static object MapRequest(HttpRequestMessage request)
        {
            if (request == null)
                return "<NULL>";

            var result = new Dictionary<string, object>();

            AddToDetailsObject(result, "Method", () => request.Method.SafeToString());
            AddToDetailsObject(result, "Uri", () => request.RequestUri);
            AddToDetailsObject(result, "Version", () => request.Version.SafeToString());
            AddToDetailsObject(result, "Headers", () => MapHeaders(request.Headers));
            AddToDetailsObject(result, "Properties", () => request.Properties.ToDictionary(p => p.Key, p => p.Value.SafeToString()));

            return result;
        }

        private static object MapRouteData(IHttpRouteData routeData)
        {
            if (routeData == null)
                return "<NULL>";

            return new Dictionary<string, object>
            {
                {"RouteTemplate", routeData.Route.IfNotNull(r => r.RouteTemplate)},
                {"Values", routeData.Values.ToDictionary(v => v.Key, v => v.Value.SafeToString())}
            };
        }

        private static object MapPrincipal(IPrincipal principal)
        {
            return new Dictionary<string, object>
            {
                {"HasPrincipal", principal != null},
                {"HasIdentity", principal.IfNotNull(p => p.Identity != null)},
                {"Principal", principal.SafeToString()},
                {"Identity", principal.IfNotNull(p => p.Identity.SafeToString())},
                {"IsAuthenticated", principal.IfNotNull(p => p.Identity.IfNotNull(i => i.IsAuthenticated))},
                {"Name", principal.IfNotNull(p => p.Identity.IfNotNull(i => i.Name))},
                {"AuthenticationType", principal.IfNotNull(p => p.Identity.IfNotNull(i => i.AuthenticationType))}
            };
        }

        private static Dictionary<string, string> MapHeaders(HttpHeaders headers)
        {
            return headers.ToDictionary(h => h.Key, h => string.Join(",", h.Value));
        }

        private static void AddToDetailsObject(IDictionary<string, object> result, string propertyName, Func<object> func)
        {
            try
            {
                result[propertyName] = func();
            }
            catch (Exception e)
            {
                result[propertyName] = "Exception occured: " + e.Message;
            }
        }
    }
}
namespace Appson.Common.Owin
{
    public static class OwinRequestScopeContextExtensions
    {
        public static string GetRequestMethod(this OwinRequestScopeContext context)
        {
            return context.IfNotNull(
                c => c.OwinContext.IfNotNull(
                    oc => oc.Request.IfNotNull(
                        r => r.Method)));
        }

        public static int GetResponseStatusCode(this OwinRequestScopeContext context)
        {
            return context.IfNotNull(
                c => c.OwinContext.IfNotNull(
                    oc => oc.Response.IfNotNull(
                        r => r.StatusCode)));
        }

        public static bool IsGet(this OwinRequestScopeContext context)
        {
            return (context.GetRequestMethod() ?? "").ToUpper().Equals("GET");
        }

        public static bool IsSuccess(this OwinRequestScopeContext context)
        {
            var status = context.GetResponseStatusCode();
            return status >= 200 && status < 300;
        }
    }
}
using System;
using System.Web;

namespace Appson.Common.Web.Extensions
{
    /// <summary>
    /// Copy of AjaxRequestExtensions from System.Web.Mvc, but replaced HttpRequestBase with HttpRequest
    /// </summary>
    public static class AjaxRequestExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return (request["X-Requested-With"] == "XMLHttpRequest") || ((request.Headers["X-Requested-With"] == "XMLHttpRequest"));
        }
    }
}

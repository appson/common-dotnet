using System.Linq;
using System.Web;
using System.Web.Security.AntiXss;
using Appson.Common.General.Text;

namespace Appson.Common.Web.Utils
{
    public static class HtmlStringUtil
	{
		public static IHtmlString JoinNonEmpty(string separator, params object[] strings)
		{
			if (strings == null)
				return null;

			var values = strings
				.Where(s => s != null)
				.Select(s => s is HtmlString ? ((HtmlString) s).ToHtmlString() : AntiXssEncoder.HtmlEncode(s.ToString(), false))
				.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

			if (values.Length == 0)
				return null;

			return new HtmlString(string.Join(separator, values));
		}

        public static HtmlString FormatAndEncodeIfNotNull(this string format, params object[] formatParams)
        {
            if (formatParams.Any(fp => fp == null))
                return null;

            return new HtmlString(string.Format(AntiXssEncoder.HtmlEncode(format, false),
                                                formatParams.Select(fp => fp is HtmlString ? fp.ToString() : AntiXssEncoder.HtmlEncode(fp.ToString(), false)).Cast<object>().ToArray()));
        }

        public static HtmlString FormatAndEncodeIfNotEmpty(this string format, params string[] formatParams)
        {
            if (formatParams.Any(string.IsNullOrWhiteSpace))
                return null;

            return new HtmlString(string.Format(AntiXssEncoder.HtmlEncode(format, false),
                                                formatParams.Select(p => AntiXssEncoder.HtmlEncode(p, false)).Cast<object>().ToArray()));
        }

        public static HtmlString FormatAndEncodeIfNotEmpty(this string format, params object[] formatParams)
        {
            if (formatParams.Any(fp => fp == null))
                return null;

            var formatParamStrings = formatParams.Select(fp => fp is HtmlString ? fp.ToString() : AntiXssEncoder.HtmlEncode(fp.ToString(), false)).ToArray();
            var result = AntiXssEncoder.HtmlEncode(format, false).FormatIfNotEmpty(formatParamStrings);

            return result == null ? null : new HtmlString(result);
        }

    }
}
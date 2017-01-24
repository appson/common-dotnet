using System.Threading;

namespace Appson.Common.Web.Utils
{
    public static class CurrentUiCulture
    {
        public static string Tag => Thread.CurrentThread.CurrentUICulture.IetfLanguageTag;

        public static string LanguageCode => Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;

        public static string NativeName => Thread.CurrentThread.CurrentUICulture.NativeName;

        public static string EnglishName => Thread.CurrentThread.CurrentUICulture.EnglishName;

        public static bool IsRightToLeft => Thread.CurrentThread.CurrentUICulture.TextInfo.IsRightToLeft;

        public static bool IsLeftToRight => !Thread.CurrentThread.CurrentUICulture.TextInfo.IsRightToLeft;

        public static string Direction => Thread.CurrentThread.CurrentUICulture.TextInfo.IsRightToLeft ? "rtl" : "ltr";

        public static string ReverseDirection => Thread.CurrentThread.CurrentUICulture.TextInfo.IsRightToLeft ? "ltr" : "rtl";

        public static string Far => Thread.CurrentThread.CurrentUICulture.TextInfo.IsRightToLeft ? "left" : "right";

        public static string Near => Thread.CurrentThread.CurrentUICulture.TextInfo.IsRightToLeft ? "right" : "left";
    }
}
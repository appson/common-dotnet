using System;
using System.Globalization;

namespace JahanJooy.Stimulsoft.Common.Functions
{
    public static class StiJalaaliDateFunctions
    {
        public static string JalaaliDateToStr(DateTime value)
        {
            var cal = new PersianCalendar();
            return string.Format("{0}/{1}/{2}", cal.GetYear(value), cal.GetMonth(value), cal.GetDayOfMonth(value));
        }
    }
}
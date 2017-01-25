using System;
using System.Globalization;

namespace Appson.Common.Stimulsoft.Functions
{
    public static class StiJalaaliDateFunctions
    {
        public static string JalaaliDateToStr(DateTime value)
        {
            var cal = new PersianCalendar();
            return $"{cal.GetYear(value)}/{cal.GetMonth(value)}/{cal.GetDayOfMonth(value)}";
        }
    }
}
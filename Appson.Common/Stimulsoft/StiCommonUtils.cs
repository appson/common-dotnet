using System;
using Appson.Common.Stimulsoft.Functions;
using Stimulsoft.Report.Dictionary;

namespace Appson.Common.Stimulsoft
{
    public static class StiCommonUtils
    {
        public static void RegisterCustomFunctions()
        {
            StiFunctions.AddFunction("Jalaali", "JalaaliDateToStr", "Converts a date to Jalaali calendar",
                typeof (StiJalaaliDateFunctions), typeof (string), "", new[] {typeof (DateTime)}, new[] {"value"});
        }
    }
}
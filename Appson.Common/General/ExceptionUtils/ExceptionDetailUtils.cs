using System;
using System.Collections;
using System.Collections.Generic;

namespace Appson.Common.General.ExceptionUtils
{
    public static class ExceptionDetailUtils
    {
        public static Dictionary<string, object> ToDetailsDictionary(this Exception exception)
        {
            if (exception == null)
                return null;

            var result = new Dictionary<string, object>();

            AddToDetailsObject(result, "Exception", () => MapException(exception, 0));

            return result;
        }

        public static object MapException(Exception e, int nestingLevel)
        {
            if (e == null)
                return "<NULL>";

            if (nestingLevel >= 8)
                return "<TRUNCATED>";

            var result = new Dictionary<string, object>();

            AddToDetailsObject(result, "Message", () => e.Message);
            AddToDetailsObject(result, "Data", () => MapExceptionData(e.Data));
            AddToDetailsObject(result, "Source", () => e.Source);
            AddToDetailsObject(result, "TargetSite", () => e.TargetSite.ToString());
            AddToDetailsObject(result, "InnerException", () => MapException(e.InnerException, nestingLevel + 1));

            return result;
        }

        private static object MapExceptionData(IDictionary data)
        {
            if (data == null)
                return "<NULL>";

            if (data.Count == 0)
                return "<EMPTY>";

            var result = new Dictionary<string, object>();
            foreach (var entry in data.Keys)
            {
                // ReSharper disable once AccessToForEachVariableInClosure
                AddToDetailsObject(result, entry.ToString(), () => data[entry].ToString());
            }

            return result;
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
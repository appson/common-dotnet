using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Web.Mvc;
using Appson.Common.General.Localization;
using Appson.Common.General.Utils;

namespace Appson.Common.Web.Extensions
{
	public static class SelectHtmlExtensions
	{
		#region SelectItem utilities

		public static IEnumerable<SelectListItem> EnumSelectList<TEnum>(this TEnum enumObj,
		                                                                ResourceManager resourceManager = null)
			where TEnum : struct
		{
			return EnumSelectListInternal<TEnum>(resourceManager);
		}

		public static IEnumerable<SelectListItem> EnumSelectList<TEnum>(this TEnum? enumObj,
		                                                                ResourceManager resourceManager = null)
			where TEnum : struct
		{
			return EnumSelectListInternal<TEnum>(resourceManager);
		}

		public static IEnumerable<SelectListItem> EnumSelectList<TEnum>(this IEnumerable<TEnum> enumItems,
		                                                                ResourceManager resourceManager = null)
			where TEnum : struct
		{
			return EnumSelectListInternal(enumItems, resourceManager);
		}

		public static IEnumerable<SelectListItem> SelectListItems<TEntity>(this IEnumerable<TEntity> source,
		                                                                   Func<TEntity, string> textSelector,
		                                                                   Func<TEntity, string> keySelector = null)
		{
			IEnumerable<SelectListItem> result =
				source.Select(e => new SelectListItem
					                   {
						                   Text = textSelector(e),
						                   Value = (keySelector ?? textSelector)(e)
					                   });

			return result;
		}

		public static IEnumerable<SelectListItem> BuildBooleanList(ResourceManager resourceManager = null)
		{
			if (resourceManager == null)
				resourceManager = UtilResources.ResourceManager;

			return new List<SelectListItem>
				       {
					       new SelectListItem {Text = resourceManager.GetString("Boolean_True"), Value = "true"},
					       new SelectListItem {Text = resourceManager.GetString("Boolean_False"), Value = "false"}
				       };
		}

	    public static IEnumerable<SelectListItem> BuildTimeOfDayList(int intervalMinutes)
	    {
            if (intervalMinutes <= 0)
                throw new ArgumentException("intervalMinutes cannot be negative or zero.");

            var result = new List<SelectListItem>();

	        for (int minutes = 0; minutes < 24*60; minutes += intervalMinutes)
	        {
	            TimeSpan ts = TimeSpan.FromMinutes(minutes);
                result.Add(new SelectListItem
                           {
                               Value = minutes.ToString(),
                               // ReSharper disable once LocalizableElement
                               Text = $"{ts.Hours:D2}:{ts.Minutes:D2}"
                           });
	        }

	        return result;
	    }

		#endregion

		#region Private helper methods

	    private static IEnumerable<SelectListItem> EnumSelectListInternal<TEnum>(ResourceManager resourceManager = null)
			where TEnum : struct
		{
			Type enumType = typeof (TEnum);
			return EnumSelectListInternal(Enum.GetValues(enumType).OfType<TEnum>(), resourceManager);
		}

	    private static IEnumerable<SelectListItem> EnumSelectListInternal<TEnum>(IEnumerable<TEnum> enumItems, ResourceManager resourceManager = null)
	        where TEnum : struct
	    {
	        IEnumerable<SelectListItem> result = enumItems.Select(e => new SelectListItem
	                                                                   {
	                                                                       Value = e.ToString(),
	                                                                       Text = e.Label(resourceManager)
	                                                                   });

	        return result;

	    }

	    #endregion
	}
}
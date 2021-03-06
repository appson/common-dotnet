﻿using System;
using System.Web.Mvc;
using Appson.Common.General.Text;

namespace Appson.Common.Web.Validation
{
	public class NumericModelBinder : IModelBinder
	{
	    private readonly Func<string, object> _convertFunc;

        public NumericModelBinder(Func<string, object> convertFunc)
	    {
            if (convertFunc == null)
                throw new ArgumentNullException(nameof(convertFunc));

	        _convertFunc = convertFunc;
	    }

	    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

	        ModelState modelState;
	        if (bindingContext.ModelState.ContainsKey(bindingContext.ModelName))
	        {
	            modelState = bindingContext.ModelState[bindingContext.ModelName];
	            modelState.Value = valueResult;
	        }
	        else
	        {
                modelState = new ModelState { Value = valueResult };
                bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            }

			if (string.IsNullOrWhiteSpace(valueResult?.AttemptedValue))
				return null;

			object actualValue = null;
			try
			{
			    actualValue = _convertFunc(DigitLocalizationUtils.ToEnglish(valueResult.AttemptedValue));
			}
			catch (FormatException e)
			{
				modelState.Errors.Add(e);
			}

			return actualValue;
		}
	}
}
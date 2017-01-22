using System;
using System.Web.Mvc;

namespace Appson.Common.Web.Attributes
{
	[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	public abstract class SkipValidationIfAttribute : Attribute
	{
		public abstract bool ShouldSkipValidation(ControllerContext controllerContext, ModelBindingContext bindingContext, ModelMetadata propertyMetadata, ModelValidationResult validationResult);
	}
}
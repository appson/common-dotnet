using System;
using System.ComponentModel;
using System.Web.Mvc;

namespace Appson.Common.Web.Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public abstract class ValidationExtensionAttribute : Attribute
	{
		public abstract void OnPropertyValidated(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value);
		public abstract bool OnPropertyValidating(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value);
	}
}
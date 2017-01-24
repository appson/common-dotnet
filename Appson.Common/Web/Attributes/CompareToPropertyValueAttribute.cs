using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace Appson.Common.Web.Attributes
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
	public class CompareToPropertyValueAttribute : ValidationAttribute
	{
		private readonly string _propertyName;
		private readonly PropertyValidationComparisonUtil.ComparisonType _comparisonType;

		public CompareToPropertyValueAttribute(PropertyValidationComparisonUtil.ComparisonType comparisonType, string propertyName)
		{
			_propertyName = propertyName;
			_comparisonType = comparisonType;

			if (propertyName == null)
				throw new ArgumentNullException(nameof(propertyName));
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			PropertyInfo property = validationContext.ObjectType.GetProperty(_propertyName);
			if (property == null)
			{
				return new ValidationResult("PropertyName is not specified.");
			}

			object targetValue = property.GetValue(validationContext.ObjectInstance, null);

			var comparisonResult = PropertyValidationComparisonUtil.Compare(value, _comparisonType, targetValue);
			if (!comparisonResult.HasValue || comparisonResult.Value)
				return null;

			return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
		}

		public override string FormatErrorMessage(string name)
		{
			return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name);
		}
	}
}
﻿using System;

namespace Appson.Common.Web.Attributes
{
	/// <summary>
	/// Validates the model field to contain (or exactly match) an international phone number.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
	public class PossiblePhoneNumberAttribute : ExtendedRegularExpressionAttribute
	{
		public PossiblePhoneNumberAttribute(bool allowPartialMatch = true)
			: base(@"(\+?(\s*\d)+)?\s*(\((\s*\d)+\))?(\s*\d){4,}(\s*\-\s*\d+)?", allowPartialMatch)
		{
		}
	}
}
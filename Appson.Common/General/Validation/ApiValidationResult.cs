using System;
using System.Collections.Generic;
using System.Linq;

namespace Appson.Common.General.Validation
{
    public class ApiValidationResult
	{
		public bool Success { get; set; }
		public List<ApiValidationError> Errors { get; set; }

		public static ApiValidationResult Ok()
		{
			return new ApiValidationResult {Success = true, Errors = null};
		}

		public static ApiValidationResult Failure(ApiValidationError error)
		{
			return new ApiValidationResult {Success = false, Errors = new List<ApiValidationError> {error}};
		}

		public static ApiValidationResult Failure(IEnumerable<ApiValidationError> errors)
		{
			return new ApiValidationResult {Success = false, Errors = errors.ToList()};
		}
		
		public static ApiValidationResult Failure(string errorKey)
		{
			return new ApiValidationResult {Success = false, Errors = new List<ApiValidationError> {new ApiValidationError(errorKey)}};
		}

		public static ApiValidationResult Failure(string errorKey, IEnumerable<string> errorParameters)
		{
			return new ApiValidationResult {Success = false, Errors = new List<ApiValidationError> {new ApiValidationError(errorKey, errorParameters)}};
		}

		public static ApiValidationResult Failure(string propertyPath, string errorKey)
		{
			return new ApiValidationResult {Success = false, Errors = new List<ApiValidationError> {new ApiValidationError(propertyPath, errorKey)}};
		}

		public static ApiValidationResult Failure(string propertyPath, string errorKey, IEnumerable<string> errorParameters)
		{
			return new ApiValidationResult {Success = false, Errors = new List<ApiValidationError> {new ApiValidationError(propertyPath, errorKey, errorParameters)}};
		}

        public ApiValidationResult Append(ApiValidationError error)
        {
            Errors.Add(error);
            return this;
        }

        public ApiValidationResult Append(string errorKey)
        {
            return Append(new ApiValidationError(errorKey));
        }

        public ApiValidationResult Append(string errorKey, IEnumerable<string> errorParameters)
        {
            return Append(new ApiValidationError(errorKey, errorParameters));
        }

        public ApiValidationResult Append(string propertyPath, string errorKey)
        {
            return Append(new ApiValidationError(propertyPath, errorKey));
        }

        public ApiValidationResult Append(string propertyPath, string errorKey, IEnumerable<string> errorParameters)
        {
            return Append(new ApiValidationError(propertyPath, errorKey, errorParameters));
        }

        public ApiValidatedResult<T> ToFailedValidatedResult<T>()
        {
            if (Success)
                throw new InvalidOperationException(
                    "This method should only be called on a ValidationResult instance that contains errors. " +
                    "Consider checking the state using IsValid property before calling this method.");

            return ApiValidatedResult<T>.Failure(Errors);
        }
    }
}
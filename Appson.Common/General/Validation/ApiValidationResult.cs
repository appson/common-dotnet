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
	}
}
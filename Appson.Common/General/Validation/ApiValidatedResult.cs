using System;
using System.Collections.Generic;
using System.Linq;

namespace Appson.Common.General.Validation
{
    public class ApiValidatedResult<T> : ApiValidationResult
	{
		public T Result { get; set; }

		public static ApiValidatedResult<T> Ok(T result)
		{
			return new ApiValidatedResult<T> { Errors = null, Result = result};
		}

        public new static ApiValidatedResult<T> Failure(IEnumerable<ApiValidationError> errors)
        {
            if (errors == null)
                throw new ArgumentNullException(nameof(errors));

            var errorList = errors.ToList();
            return new ApiValidatedResult<T> { Errors = errorList };
        }

        public new static ApiValidatedResult<T> Failure(ApiValidationError error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            return new ApiValidatedResult<T> { Errors = new List<ApiValidationError> { error } };
        }

        public new static ApiValidatedResult<T> Failure(string errorKey)
        {
            return Failure(new ApiValidationError(errorKey));
        }

        public new static ApiValidatedResult<T> Failure(string errorKey, IEnumerable<string> errorParameters)
        {
            return Failure(new ApiValidationError(errorKey, errorParameters));
        }

        public new static ApiValidatedResult<T> Failure(string propertyPath, string errorKey)
        {
            return Failure(new ApiValidationError(propertyPath, errorKey));
        }

        public new static ApiValidatedResult<T> Failure(string propertyPath, string errorKey, IEnumerable<string> errorParameters)
        {
            return Failure(new ApiValidationError(propertyPath, errorKey, errorParameters));
        }
	}
}
using FluentValidation;
using Identity.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Middlewares
{
    public class ValidationExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ValidationExceptionHandler> _logger;

        public ValidationExceptionHandler(ILogger<ValidationExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var problemDetails = exception switch
            {
                CustomValidationException customValidationEx => new ValidationProblemDetails(customValidationEx.Errors)
                {
                    Title = "Validation error",
                    Detail = customValidationEx.Message,
                    Status = StatusCodes.Status400BadRequest,
                },
                ValidationException validationEx => new ValidationProblemDetails(
                    validationEx.Errors.GroupBy(
                        x => x.PropertyName,
                        x => x.ErrorMessage,
                        (propertyName, errorMessages) => new
                        {
                            Key = propertyName,
                            Value = errorMessages.Distinct().ToArray(),
                        })
                    .ToDictionary(x => x.Key, x => x.Value))
                {
                    Title = "Validation error",
                    Detail = validationEx.Message,
                    Status = StatusCodes.Status400BadRequest,
                },
                _ => null
            };

            if (problemDetails is null) return false;

            _logger.LogError(exception, "Validation exception occurred:{message}", problemDetails.Detail);

            httpContext.Response.StatusCode = problemDetails!.Status!.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);

            return true;
        }
    }
}

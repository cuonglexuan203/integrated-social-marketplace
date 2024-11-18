using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Feed.API.Middlewares
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
            if (exception is not ValidationException validationException)
                return false;
            var problemDetails = new ValidationProblemDetails(
                    validationException.Errors.GroupBy(
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
                Detail = validationException.Message,
                Status = StatusCodes.Status400BadRequest,
            };

            if (problemDetails is null) return false;

            _logger.LogError(exception, "Validation exception occurred:{message}", problemDetails.Detail);

            httpContext.Response.StatusCode = problemDetails!.Status!.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);

            return true;
        }
    }
}

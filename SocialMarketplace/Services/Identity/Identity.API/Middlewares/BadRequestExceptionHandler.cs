using Identity.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Middlewares
{
    public class BadRequestExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<BadRequestExceptionHandler> _logger;

        public BadRequestExceptionHandler(ILogger<BadRequestExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not BadRequestException badRequestException)
                return false;

            _logger.LogError(badRequestException, "Bad request exception occurred: {message}", badRequestException.Message);

            var problemDetails = new ProblemDetails()
            {
                Title = "Bad request exception",
                Detail = badRequestException.Message,
                Status = StatusCodes.Status400BadRequest,
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response.WriteAsJsonAsync(problemDetails).ConfigureAwait(false);

            return true;
        }
    }
}

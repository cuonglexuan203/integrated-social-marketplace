using Microsoft.AspNetCore.Identity;
using FluentValidation.Results;

namespace Identity.Application.Exceptions
{
    public class CustomValidationException : Exception
    {
        public Dictionary<string, string[]> Errors { get; }

        public CustomValidationException()
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }
        public CustomValidationException(string message)
            : base(message)
        {
            Errors = new Dictionary<string, string[]>();
        }
        public CustomValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );
        }

        public override string ToString()
        {
            return $"Validation failed: {string.Join(", ", Errors.Select(e => $"{e.Key}: {string.Join(", ", e.Value)}"))}";
        }

        public CustomValidationException(IEnumerable<IdentityError> errors) : this()
        {
            Errors = errors
                .GroupBy(e => e.Code)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.Description).ToArray()
                );
        }

    }
}

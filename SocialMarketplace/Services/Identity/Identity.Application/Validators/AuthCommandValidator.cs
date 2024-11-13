using FluentValidation;
using Identity.Application.Commands.Auth;

namespace Identity.Application.Validators
{
    public class AuthCommandValidator : AbstractValidator<AuthCommand>
    {
        public AuthCommandValidator()
        {
            RuleFor(a => a.UserName).NotNull().NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(a => a.Password).NotNull().NotEmpty().MinimumLength(6).WithMessage("{PropertyName} length is at least 6");
        }
    }
}

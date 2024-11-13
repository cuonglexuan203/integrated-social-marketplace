using FluentValidation;
using Identity.Application.Commands.User.Create;
using Identity.Core.Enums;

namespace Identity.Application.Validators
{
    public class CreateUserCommandValidator: AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(a => a.UserName).NotNull().NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(a => a.Password).NotNull().NotEmpty().MinimumLength(6).WithMessage("{PropertyName} length is at least 6");
            RuleFor(a => a.FullName).NotNull().NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(a => a.ConfirmationPassword).Equal(a => a.Password).WithMessage("{PropertyName} must be the same password");
            RuleFor(a => a.Email).EmailAddress().When(a => !string.IsNullOrEmpty(a.Email)).WithMessage("The email must be valid if provided.");
            RuleForEach(a => a.Roles).Must(role => Enum.TryParse<SMRole>(role, out _)).WithMessage("Each role must be a valid value.");
        }
    }
}

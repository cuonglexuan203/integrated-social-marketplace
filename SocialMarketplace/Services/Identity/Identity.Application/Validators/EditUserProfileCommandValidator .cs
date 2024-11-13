using FluentValidation;
using Identity.Application.Commands.User.Update;
using Identity.Core.Enums;

namespace Identity.Application.Validators
{
    public class EditUserProfileCommandValidator : AbstractValidator<EditUserProfileCommand>
    {
        public EditUserProfileCommandValidator()
        {
            RuleFor(a => a.Id)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} is required");

            RuleFor(a => a.FullName)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} is required");

            RuleFor(a => a.Email)
                .EmailAddress()
                .When(a => !string.IsNullOrEmpty(a.Email))
                .WithMessage("The email must be valid if provided.");

            RuleForEach(a => a.Roles)
                .Must(role => Enum.TryParse<SMRole>(role, out _))
                .WithMessage("Each role must be a valid value from SMRole.");
        }
    }
}

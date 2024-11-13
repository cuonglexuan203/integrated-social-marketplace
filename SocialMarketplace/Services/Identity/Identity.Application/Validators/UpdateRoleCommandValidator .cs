using FluentValidation;
using Identity.Application.Commands.Role.Update;
using Identity.Core.Enums;

namespace Identity.Application.Validators
{
    public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} is required.");

            RuleFor(x => x.RoleName).Must(role => Enum.TryParse<SMRole>(role, out _)).WithMessage("Role name must be a valid value.");
        }
    }
}

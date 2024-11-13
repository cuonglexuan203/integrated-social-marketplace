using FluentValidation;
using Identity.Application.Commands.Role.Create;
using Identity.Core.Enums;

namespace Identity.Application.Validators
{
    public class RoleCreateCommandValidator : AbstractValidator<RoleCreateCommand>
    {
        public RoleCreateCommandValidator()
        {
            RuleFor(x => x.RoleName).NotEmpty().WithMessage("Role name is required.");
            RuleFor(x => x.RoleName).Must(role => Enum.TryParse<SMRole>(role, out _)).WithMessage("Role name must be a valid value.");
        }
    }
}

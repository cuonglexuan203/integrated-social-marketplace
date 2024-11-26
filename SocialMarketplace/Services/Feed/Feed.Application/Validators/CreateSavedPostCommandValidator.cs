
using Feed.Application.Commands;
using Feed.Application.Interfaces;
using FluentValidation;

namespace Feed.Application.Validators
{
    public class CreateSavedPostCommandValidator: AbstractValidator<CreateSavedPostCommand>
    {
        public CreateSavedPostCommandValidator(IMongoIdValidator mongoIdValidator)
        {
            RuleFor(x => x.PostId)
                .NotNull()
                .NotEmpty()
                .Must(mongoIdValidator.IsValid)
                .WithMessage("Invalid {PropertyName} format");
        }
    }
}

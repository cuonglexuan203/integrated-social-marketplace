using Feed.Application.Commands;
using Feed.Application.Interfaces;
using FluentValidation;

namespace Feed.Application.Validators
{
    public class UnsavedPostCommandValidator : AbstractValidator<UnsavePostCommand>
    {
        public UnsavedPostCommandValidator(IMongoIdValidator mongoIdValidator)
        {
            RuleFor(x => x.PostId)
                .NotNull()
                .NotEmpty()
                .Must(mongoIdValidator.IsValid)
                .WithMessage("Invalid {PropertyName} format");
        }
    }
}

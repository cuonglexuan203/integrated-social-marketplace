using Feed.Application.Commands;
using Feed.Application.Interfaces;
using Feed.Core.Enums;
using FluentValidation;

namespace Feed.Application.Validators
{
    public class AddReactionToPostCommandValidator : AbstractValidator<AddReactionToPostCommand>
    {
        public AddReactionToPostCommandValidator(IMongoIdValidator mongoIdValidator)
        {
            RuleFor(x => x.PostId)
                .NotNull()
                .NotEmpty()
                .Must(mongoIdValidator.IsValid)
                .WithMessage("Invalid {PropertyName} format");
            RuleFor(x => x.UserId).NotNull().NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(x => x.ReactionType)
                .Must(value => Enum.IsDefined(typeof(ReactionType), value))
                .WithMessage("{PropertyName} value is not a valid reaction type");
        }
    }
}

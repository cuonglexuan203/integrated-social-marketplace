using Feed.Application.Commands;
using Feed.Core.Enums;
using FluentValidation;

namespace Feed.Application.Validators
{
    public class AddReactionToPostValidator: AbstractValidator<AddReactionToPostCommand>
    {
        public AddReactionToPostValidator()
        {
            RuleFor(x => x.PostId).NotNull().NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(x => x.UserId).NotNull().NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(x => x.ReactionType)
                .Must(value => Enum.IsDefined(typeof(ReactionType),value))
                .WithMessage("{PropertyName} value is not a valid reaction type");
        }
    }
}

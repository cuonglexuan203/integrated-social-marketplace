using Feed.Application.Commands;
using Feed.Application.Interfaces;
using FluentValidation;

namespace Feed.Application.Validators
{
    public class RemoveReactionFromCommentCommandValidator : AbstractValidator<RemoveReactionFromCommentCommand>
    {
        public RemoveReactionFromCommentCommandValidator(IMongoIdValidator mongoIdValidator)
        {
            RuleFor(x => x.CommentId)
                .NotNull()
                .NotEmpty()
                .Must(mongoIdValidator.IsValid)
                .WithMessage("Invalid {PropertyName} format");
        }
    }
}

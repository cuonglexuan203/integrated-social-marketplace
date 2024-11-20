
using Feed.Application.Commands;
using Feed.Application.Interfaces;
using FluentValidation;

namespace Feed.Application.Validators
{
    public class AddReactionToCommentCommandValidator: AbstractValidator<AddReactionToCommentCommand>
    {
        public AddReactionToCommentCommandValidator(IMongoIdValidator mongoIdValidator)
        {
            RuleFor(x => x.CommentId)
                .NotNull()
                .NotEmpty()
                .Must(mongoIdValidator.IsValid)
                .WithMessage("Invalid {PropertyName} format");
        }
    }
}

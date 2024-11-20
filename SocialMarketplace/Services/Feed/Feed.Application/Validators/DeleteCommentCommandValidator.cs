using Feed.Application.Commands;
using Feed.Application.Interfaces;
using FluentValidation;

namespace Feed.Application.Validators
{
    public class DeleteCommentCommandValidator: AbstractValidator<DeleteCommentCommand>
    {
        public DeleteCommentCommandValidator(IMongoIdValidator mongoIdValidator)
        {
            RuleFor(x => x.CommentId)
                .NotNull()
                .NotEmpty()
                .Must(mongoIdValidator.IsValid)
                .WithMessage("Invalid {PropertyName} format");
        }
    }
}

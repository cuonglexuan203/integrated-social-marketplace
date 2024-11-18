using Feed.Application.Commands;
using FluentValidation;

namespace Feed.Application.Validators
{
    public class CreatePostCommandValidator: AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotNull().WithMessage("{PropertyName} is required");
        }
    }
}

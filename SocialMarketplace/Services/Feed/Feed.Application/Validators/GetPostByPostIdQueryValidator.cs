using Feed.Application.Interfaces;
using Feed.Application.Queries;
using FluentValidation;

namespace Feed.Application.Validators
{
    public class GetPostByPostIdQueryValidator: AbstractValidator<GetPostByPostIdQuery>
    {
        public GetPostByPostIdQueryValidator(IMongoIdValidator mongoIdValidator)
        {
            RuleFor(x => x.PostId)
                .NotNull()
                .NotEmpty()
                .Must(mongoIdValidator.IsValid)
                .WithMessage("Invalid {PropertyName} format");
        }
    }
}

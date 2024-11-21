using Identity.Application.Interfaces;
using Identity.Core.Enums;
using MediatR;

namespace Identity.Application.Commands.User.Create
{
    public class CreateUserCommand : IRequest<int>
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmationPassword { get; set; }
        public ICollection<string> Roles { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public int? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public ICollection<string>? Interests { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IIdentityService _identityService;
        public CreateUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.CreateUserAsync(request);
            return result.isSucceed ? 1 : 0;
        }
    }
}

using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Commands.Auth
{
    public class LogoutCommand : IRequest<Unit>
    {

    }

    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
    {
        private readonly IIdentityService _identityService;

        public LogoutCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            return Unit.Value;
        }
    }
}

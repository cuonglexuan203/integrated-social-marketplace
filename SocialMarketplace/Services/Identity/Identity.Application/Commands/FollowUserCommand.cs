using MediatR;

namespace Identity.Application.Commands
{
    public class FollowUserCommand : IRequest<bool>
    {
        public string FollowerId { get; set; } = default!;
        public string FollowedId { get; set; } = default!;
    }
    public class FollowUserCommandHandler : IRequestHandler<FollowUserCommand, bool>
    {
        private readonly IdentityContext _context;

        public FollowUserCommandHandler(IdentityContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(FollowUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.FollowerId) || string.IsNullOrEmpty(request.FollowedId))
                {
                    return Result.Failure("Invalid user IDs");
                }

                if (request.FollowerId == request.FollowedId)
                {
                    return Result.Failure("Users cannot follow themselves");
                }

                var follower = await _context.Users.FindAsync(new object[] { request.FollowerId }, cancellationToken);
                var followed = await _context.Users.FindAsync(new object[] { request.FollowedId }, cancellationToken);

                if (follower == null || followed == null)
                {
                    return Result.Failure("One or both users not found");
                }

                var existingFollow = await _context.UserFollows
                    .FirstOrDefaultAsync(uf =>
                        uf.FollowerId == request.FollowerId &&
                        uf.FollowedId == request.FollowedId,
                        cancellationToken);

                if (existingFollow != null)
                {
                    return Result.Failure("Already following this user");
                }

                var userFollow = new UserFollow
                {
                    Id = Guid.NewGuid().ToString(),
                    FollowerId = request.FollowerId,
                    FollowedId = request.FollowedId,
                    CreatedAt = DateTimeOffset.UtcNow
                };

                _context.UserFollows.Add(userFollow);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}

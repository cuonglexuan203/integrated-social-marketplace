namespace Chat.Core.Interfaces
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        string Username { get; }
        //string Email { get; }
        IEnumerable<string> Roles { get; }
        bool IsAuthenticated { get; }
        string JwtToken { get; }
    }
}

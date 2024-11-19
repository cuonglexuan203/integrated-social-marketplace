
using Feed.Core.Entities;

namespace Feed.Application.Interfaces.HttpClients
{
    public interface IIdentityService
    {
        Task<CompactUser> GetUserDetailsAsync(string userId);
    }
}

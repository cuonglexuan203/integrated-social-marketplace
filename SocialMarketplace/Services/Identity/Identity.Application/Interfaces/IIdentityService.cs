using Identity.Application.Commands.User.Create;
using Identity.Application.Commands.User.Update;
using Identity.Application.DTOs;
using Identity.Core.Specs;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Interfaces
{
    public interface IIdentityService
    {
        // Populate data
        Task PopulateUserData(IEnumerable<LoadExistingUserCommand> commands);
        Task<Pagination<UserDetailsResponseDTO>> SearchUserFullName(UserSpecParams userSpecParams);
        // User section
        Task<(bool isSucceed, string userId)> CreateUserAsync(string userName, string password, string email, string fullName, List<string> roles);
        Task<(bool isSucceed, string userId)> CreateUserAsync(CreateUserCommand user);
        Task<bool> SigninUserAsync(string userName, string password);
        //Task<bool> SignOutAsync();
        Task<string> GetUserIdAsync(string userName);
        Task<UserDetailsResponseDTO> GetUserDetailsAsync(string userId);
        Task<UserDetailsResponseDTO> GetUserDetailsByUserNameAsync(string userName);
        Task<string> GetUserNameAsync(string userId);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> IsUniqueUserName(string userName);
        Task<IList<UserResponseDTO>> GetAllUsersAsync();
        Task<List<(string id, string userName, string email, IList<string> roles)>> GetAllUsersDetailsAsync(); // not implement
        Task<bool> UpdateUserProfile(string id, string fullName, string email, IList<string> roles);
        Task<(IdentityResult, UserDetailsResponseDTO?)> UpdateUserProfileAsync(string userId, EditUserProfileCommand command, CancellationToken token = default);
        Task<IdentityResult> ChangePasswordAsync(string userName, string currentPassword, string newPassword, CancellationToken token = default);

        // Role Section
        Task<bool> CreateRoleAsync(string roleName);
        Task<bool> DeleteRoleAsync(string roleId);
        Task<List<(string id, string roleName)>> GetRolesAsync();
        Task<(string id, string roleName)> GetRoleByIdAsync(string id);
        Task<bool> UpdateRole(string id, string roleName);

        // User's Role section
        Task<bool> IsInRoleAsync(string userId, string role);
        Task<List<string>> GetUserRolesAsync(string userId);
        Task<bool> AssignUserToRole(string userName, IList<string> roles);
        Task<bool> UpdateUsersRole(string userName, IList<string> usersRole);
    }
}

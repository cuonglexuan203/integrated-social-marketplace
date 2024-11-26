using Identity.Application.Commands.User.Create;
using Identity.Application.Commands.User.Update;
using Identity.Application.DTOs;
using Identity.Application.Exceptions;
using Identity.Application.Interfaces;
using Identity.Core.Enums;
using Identity.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Identity.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<bool> AssignUserToRole(string userName, IList<string> roles)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            var result = await _userManager.AddToRolesAsync(user, roles);
            return result.Succeeded;
        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            var result = await _roleManager.CreateAsync(new ApplicationRole(roleName));
            if (!result.Succeeded)
            {
                throw new CustomValidationException(result.Errors);
            }
            return result.Succeeded;
        }


        // Return multiple value
        public async Task<(bool isSucceed, string userId)> CreateUserAsync(string userName, string password, string email, string fullName, List<string> roles)
        {
            var user = new ApplicationUser()
            {
                FullName = fullName,
                UserName = userName,
                Email = email
            };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    throw new NotFoundException($"Role {role} not found");
                }
            }

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new CustomValidationException(result.Errors);
            }

            var addUserRole = await _userManager.AddToRolesAsync(user, roles);
            if (!addUserRole.Succeeded)
            {
                throw new CustomValidationException(addUserRole.Errors);
            }
            return (result.Succeeded, user.Id);
        }

        public async Task<(bool isSucceed, string userId)> CreateUserAsync(CreateUserCommand command)
        {
            // Validate roles first
            foreach (var role in command.Roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    throw new NotFoundException($"Role {role} not found");
                }
            }

            // Create user with optional properties
            var user = new ApplicationUser
            {
                FullName = command.FullName,
                UserName = command.UserName,
                Email = command.Email,
                PhoneNumber = command.PhoneNumber,
                Gender = command.Gender.HasValue ? (Gender)command.Gender.Value : Gender.Unknown,
                DateOfBirth = command.DateOfBirth,
                Interests = command.Interests?.ToList() ?? new List<string>(),
                City = command.City,
                Country = command.Country
            };

            if (command.ProfilePictureUrl != null)
            {
                user.ProfilePictureUrl = command.ProfilePictureUrl;
            }

            // Create user
            var result = await _userManager.CreateAsync(user, command.Password);
            if (!result.Succeeded)
            {
                throw new CustomValidationException(result.Errors);
            }

            // Add user to roles
            var addUserRole = await _userManager.AddToRolesAsync(user, command.Roles);
            if (!addUserRole.Succeeded)
            {
                throw new CustomValidationException(addUserRole.Errors);
            }

            return (result.Succeeded, user.Id);
        }

        public async Task<bool> DeleteRoleAsync(string roleId)
        {
            var roleDetails = await _roleManager.FindByIdAsync(roleId);
            if (roleDetails == null)
            {
                throw new NotFoundException("Role not found");
            }

            if (roleDetails.Name == "Admin")
            {
                throw new BadRequestException("You can not delete Administrator Role");
            }
            var result = await _roleManager.DeleteAsync(roleDetails);
            if (!result.Succeeded)
            {
                throw new CustomValidationException(result.Errors);
            }
            return result.Succeeded;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
                //throw new Exception("User not found");
            }

            if (user.UserName == "system" || user.UserName == "admin")
            {
                throw new Exception("You can not delete system or admin user");
                //throw new BadRequestException("You can not delete system or admin user");
            }
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<IList<UserResponseDTO>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            return users.Select(user => new UserResponseDTO()
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ProfilePictureUrl = user.ProfilePictureUrl,
                ProfileUrl = user.ProfileUrl,
                DateOfBirth = user.DateOfBirth,
                Interests = user.Interests,
                City = user.City,
                Country = user.Country,
            }).ToList();
        }

        public Task<List<(string id, string userName, string email, IList<string> roles)>> GetAllUsersDetailsAsync()
        {
            throw new NotImplementedException();

            //var roles = await _userManager.GetRolesAsync(user);
            //return (user.Id, user.UserName, user.Email, roles);

            //var users = _userManager.Users.ToListAsync();
        }

        public async Task<List<(string id, string roleName)>> GetRolesAsync()
        {
            var roles = await _roleManager.Roles.Select(x => new
            {
                x.Id,
                x.Name
            }).ToListAsync();

            return roles.Select(role => (role.Id, role.Name)).ToList();
        }

        public async Task<UserDetailsResponseDTO> GetUserDetailsAsync(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            var roles = await _userManager.GetRolesAsync(user);

            return new UserDetailsResponseDTO()
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = roles,
                ProfilePictureUrl = user.ProfilePictureUrl,
                ProfileUrl = user.ProfileUrl,
                DateOfBirth = user.DateOfBirth,
                Interests = user.Interests,
                City = user.City,
                Country = user.Country,
            };
        }

        public async Task<UserDetailsResponseDTO> GetUserDetailsByUserNameAsync(string userName)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            var roles = await _userManager.GetRolesAsync(user);
            return new UserDetailsResponseDTO()
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = roles,
                ProfilePictureUrl = user.ProfilePictureUrl,
                ProfileUrl = user.ProfileUrl,
                DateOfBirth = user.DateOfBirth,
                Interests = user.Interests,
                City = user.City,
                Country = user.Country,
            };
        }

        public async Task<string> GetUserIdAsync(string userName)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                throw new NotFoundException("User not found");
                //throw new Exception("User not found");
            }
            return await _userManager.GetUserIdAsync(user);
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
                //throw new Exception("User not found");
            }
            return await _userManager.GetUserNameAsync(user);
        }

        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<bool> IsUniqueUserName(string userName)
        {
            return await _userManager.FindByNameAsync(userName) == null;
        }

        public async Task<bool> SigninUserAsync(string userName, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(userName, password, true, false);
            return result.Succeeded;


        }

        //public async Task<bool> SignOutAsync()
        //{
        //    try
        //    {
        //        await _signInManager.SignOutAsync();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        public async Task<bool> UpdateUserProfile(string id, string fullName, string email, IList<string> roles)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.FullName = fullName;
            user.Email = email;
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<(string id, string roleName)> GetRoleByIdAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            return (role.Id, role.Name);
        }

        public async Task<bool> UpdateRole(string id, string roleName)
        {
            if (roleName != null)
            {
                var role = await _roleManager.FindByIdAsync(id);
                role.Name = roleName;
                var result = await _roleManager.UpdateAsync(role);
                return result.Succeeded;
            }
            return false;
        }

        public async Task<bool> UpdateUsersRole(string userName, IList<string> usersRole)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var existingRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, existingRoles);
            result = await _userManager.AddToRolesAsync(user, usersRole);

            return result.Succeeded;
        }

        public async Task<IdentityResult> ChangePasswordAsync(string userName, string currentPassword, string newPassword, CancellationToken token = default)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if(user == null) return IdentityResult.Failed(new IdentityError() { Code = "UserNotFound", Description = "Username not found" });

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            return result;
        }

        public async Task<(IdentityResult, UserDetailsResponseDTO?)> UpdateUserProfileAsync(string userId, EditUserProfileCommand command, CancellationToken token = default)
        {

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new NotFoundException($"UserId {userId} not found");
            if (!string.IsNullOrEmpty(command.FullName))
            {
                user.FullName = command.FullName;
            }

            if (!string.IsNullOrEmpty(command.Email))
            {
                user.Email = command.Email;
            }

            if (!string.IsNullOrEmpty(command.PhoneNumber))
            {
                user.PhoneNumber = command.PhoneNumber;
            }

            if (!string.IsNullOrEmpty(command.ProfilePictureUrl))
            {
                user.ProfilePictureUrl = command.ProfilePictureUrl;
            }

            if (command.Gender.HasValue)
            {
                user.Gender = (Gender)command.Gender;
            }

            if (command.DateOfBirth.HasValue)
            {
                user.DateOfBirth = command.DateOfBirth.Value;
            }

            if (command.Interests != null && command.Interests.Any())
            {
                user.Interests = command.Interests.ToList();
            }

            if (!string.IsNullOrEmpty(command.City))
            {
                user.City = command.City;
            }

            if (!string.IsNullOrEmpty(command.Country))
            {
                user.Country = command.Country;
            }

            var result = await _userManager.UpdateAsync(user);
            UserDetailsResponseDTO userDetailsDto = default;
            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDetailsDto = new UserDetailsResponseDTO()
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles,
                    ProfilePictureUrl = user.ProfilePictureUrl,
                    ProfileUrl = user.ProfileUrl,
                    DateOfBirth = user.DateOfBirth,
                    Interests = user.Interests,
                    City = user.City,
                    Country = user.Country,
                };
            }

            return (result, userDetailsDto);

        }
    }
}

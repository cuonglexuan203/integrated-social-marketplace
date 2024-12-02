using Identity.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    public class DeveloperController: ApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DeveloperController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(_userManager.Users.ToList());
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using UserManagment.ApiContracts.User;
using UserManagment.Services;
using UserManagment.Utility;

namespace UserManagment.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateInfo createInfo)
        {
            return Ok(await _userService.CreateUser(createInfo));
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UserManagment.ApiContracts.User;
using UserManagment.Exceptions;
using UserManagment.Models;
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
        private void AssignToken(string token)
        {
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(IJwtProvider provider,[FromQuery] string login, [FromQuery] string password)
        {
            var res = await _userService.GetAuthInfo(login, password);
            if(res == null) 
                return new UnauthorizedResult();
            var token = provider.GenerateToken(res.Login, res.IsAdmin);
            AssignToken(token);
            return Ok(token);
        }
        [HttpGet("test")]
        [Authorize]
        public IActionResult Hello(ICurrentActorService currentActorService)
        {
            return Ok(currentActorService.GetLogin());
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateInfo createInfo)
        {
            return Ok(await _userService.CreateUser(createInfo));
        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteUser([FromQuery] string login, [FromQuery] bool soft)
        {
            if(soft)
                return Ok(await _userService.DeleteUserSoft(login));
            return Ok(await _userService.DeleteUserStrict(login));
        }
        [HttpGet("list/active")]
        [Authorize]
        public async Task<IActionResult> GetActiveUsers()
        {
           return Ok(await _userService.GetActiveUsers());
        }
        [HttpGet("full")]
        [Authorize]
        public async Task<IActionResult> GetUserFullInfo([FromQuery]string login, [FromQuery]string password)
        {
            var res = await _userService.GetUserFullInfo(login, password);
            if (res == null)
                return NotFound();
            return Ok(res);
        }
        [HttpGet("present")]
        [Authorize]
        public async Task<IActionResult> GetUserInfoByLogin([FromQuery]string login)
        {
            var res = await _userService.GetUserInfoByLogin(login);
            if (res == null)
                return NotFound();
            return Ok(res);
        }
        [HttpGet("list/older")]
        [Authorize]
        public async Task<IActionResult> GetUsersOlderThanAge([FromQuery] int age)
        {
            if(age <= 0)
                return BadRequest("Negative age");
            return Ok(await _userService.GetUsersOlderThanAge(age));
        }
        [HttpGet("recover")]
        [Authorize]
        public async Task<IActionResult> Recover([FromQuery]string login)
        {
            return Ok(await _userService.Recover(login));
        }
        [HttpPatch("edit/login")]
        [Authorize]
        public async Task<IActionResult> UpdateLogin([FromQuery]string userLogin, [FromQuery]string newLogin)
        {
            if (!ModelValidator.IsLoginValid(newLogin))
                return BadRequest("Invalid new login");
            return Ok(await _userService.UpdateLogin(userLogin, newLogin));
        }
        [HttpPatch("edit/password")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword([FromQuery] string userLogin, [FromQuery] string password)
        {
            if (!ModelValidator.IsPasswordValid(password))
                return BadRequest("Invalid new login");
            return Ok(await _userService.UpdatePassword(userLogin, password));
        }
        [HttpPatch("edit")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromQuery]string userLogin, [FromBody]UserUpdateInfo updateInfo)
        {
            return Ok(await _userService.UpdateUser(userLogin, updateInfo));
        }
    }
}

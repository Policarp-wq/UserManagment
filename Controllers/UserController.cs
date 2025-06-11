using Microsoft.AspNetCore.Mvc;

namespace UserManagment.Controllers
{
    public class UserController : BaseController
    {
        public UserController()
        {

        }
        [HttpGet("hello/{name}")]
        [ProducesResponseType<string>(StatusCodes.Status200OK)]
        public IActionResult GetHello(string name)
        {
            return Ok(name);
        }
    }
}

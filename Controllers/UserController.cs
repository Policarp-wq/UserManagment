using Microsoft.AspNetCore.Mvc;

namespace UserManagment.Controllers
{
    [ApiController]
    [Route("/api/v1/user/")]
    public class UserController : Controller
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

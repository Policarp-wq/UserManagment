using Microsoft.AspNetCore.Mvc;

namespace UserManagment.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]/")]
    public class BaseController : Controller
    {
        public BaseController()
        {
            
        }
    }
}

using System.Security.Claims;
using UserManagment.Exceptions;

namespace UserManagment.Services
{
    public class CurrentActorService : ICurrentActorService
    {
        public readonly string Login;
        public readonly string Role;
        public bool IsValid => !Login.Equals("undefined") && !Role.Equals("undefined");
        public CurrentActorService(IHttpContextAccessor httpContextAccessor)
        {
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                Login = "undefined";
                Role = "undefined";
                return;
            }
            Login = httpContext.User.FindFirstValue(JwtProvider.LOGIN_CLAIM) ?? "undefined";
            Role = httpContext.User.FindFirstValue(JwtProvider.ROLE_CLAIM) ?? "undefined";
        }
        public string GetLogin() => IsValid ? Login : throw new ServerException("Actor is not specified", StatusCodes.Status500InternalServerError);
        public bool IsAdmin() => IsValid ? Role.Equals(JwtProvider.ADMIN_ROLE) : throw new ServerException("Actor is not specified", StatusCodes.Status500InternalServerError);
    }
}

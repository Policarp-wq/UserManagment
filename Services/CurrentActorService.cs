using System.Security.Claims;
using UserManagment.Exceptions;

namespace UserManagment.Services
{
    public class CurrentActorService : ICurrentActorService
    {
        private const string LoginClaimType = "Login";
        private const string AdminRole = "Admin";
        public readonly string Login;
        public readonly string Role;
        public bool IsValid => !Login.Equals("undefined") && !Role.Equals("undefined");
        public CurrentActorService(HttpContext httpContext)
        {
            Login = httpContext.User.FindFirstValue(LoginClaimType) ?? "undefined";
            Role = httpContext.User.FindFirstValue(ClaimTypes.Role) ?? "undefined";
        }
        public string GetLogin() => IsValid ? Login : throw new ServerException("Actor is not specified", StatusCodes.Status500InternalServerError);
        public bool IsAdmin() => IsValid ? Role.Equals(AdminRole) : throw new ServerException("Actor is not specified", StatusCodes.Status500InternalServerError);
    }
}

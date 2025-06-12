using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagment.Models;
using UserManagment.Utility;

namespace UserManagment.Services
{
    public class JwtProvider : IJwtProvider
    {
        private const string USER_ROLE = "User";
        public static readonly string LOGIN_CLAIM = "Login";
        public static readonly string ROLE_CLAIM = "IsAdmin";
        public static readonly string ADMIN_ROLE = "Admin";
        private readonly JwtOptions _options;
        public JwtProvider(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }
        public string GenerateToken(string login, bool isAdmin)
        {
            var claims = new Claim[]
            {
                new(LOGIN_CLAIM, login),
                new(ROLE_CLAIM, isAdmin ? ADMIN_ROLE : USER_ROLE)
            };
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(
              Encoding.UTF8.GetBytes(_options.SecretKey)),
              SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _options.KeyIssuer,
                _options.Audience,
                claims,
                null,
                null,
                signingCredentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

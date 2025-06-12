using Microsoft.Extensions.Options;

namespace UserManagment.Utility
{
    public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
    {
        public const string JWT_SECTION = "JwtSection";
        private readonly IConfiguration _config;
        public JwtOptionsSetup(IConfiguration config) 
        {
            _config = config;
        }
        public void Configure(JwtOptions options)
        {
            _config.GetSection(JWT_SECTION).Bind(options);
        }
    }
}

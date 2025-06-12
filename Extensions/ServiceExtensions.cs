using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using UserManagment.Repositories;
using UserManagment.Services;
using UserManagment.Utility;

namespace UserManagment.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocs(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "User management API",
                    Version = "v1",

                });
                var securityScheme = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    securityScheme,
                    []
                    }
                });
            });
            return services;
        }
        public static IServiceCollection AddJwtAuth(this IServiceCollection services, JwtOptions options)
        {
            services.ConfigureOptions<JwtOptionsSetup>();
            services.AddSingleton<IJwtProvider, JwtProvider>();
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = options.KeyIssuer,
                        ValidateAudience = true,
                        ValidAudience = options.Audience,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = false,
                        IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(options.SecretKey))
                    };
                });
            services.AddAuthorization();
            return services;
        }
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICurrentActorService, CurrentActorService>();
            return services;
        }
    }
}

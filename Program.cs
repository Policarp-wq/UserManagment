using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using UserManagment;
using UserManagment.ApiContracts.User;
using UserManagment.Repositories;
using UserManagment.Services;
using UserManagment.Utility;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICurrentActorService, CurrentActorService>();

builder.Services.AddControllers();
//builder.Services.AddOpenApi();
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
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

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite("Data Source=user.db:memory"));

builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.AddSingleton<IJwtProvider, JwtProvider>();
var options = builder.Configuration.GetSection(JwtOptionsSetup.JWT_SECTION).Get<JwtOptions>()!;

builder.Services
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
builder.Services.AddAuthorization();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.MapOpenApi();
    app.UseSwaggerUI(opt => 
    {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "User managment API");
    } );
}
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
    var createInfo = new UserCreateInfo("admin", "admin228", "admin", 2, null, true);
    await scope.ServiceProvider.GetService<IUserRepository>()!.CreateUser(createInfo, "system");
    Console.WriteLine($"\n\nAdmin user was created with login: {createInfo.Login} | password: {createInfo.Pasword}\n\n");
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();


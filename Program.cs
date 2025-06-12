using Microsoft.EntityFrameworkCore;
using UserManagment;
using UserManagment.ApiContracts.User;
using UserManagment.Extensions;
using UserManagment.Repositories;
using UserManagment.Services;
using UserManagment.Utility;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterServices();

builder.Services.AddControllers();
//builder.Services.AddOpenApi();
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GloabalExceptionHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocs();

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite("Data Source=user.db:memory"));

var options = builder.Configuration.GetSection(JwtOptionsSetup.JWT_SECTION).Get<JwtOptions>()!;
builder.Services.AddJwtAuth(options);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.MapOpenApi();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "User managment API");
    });
}
//Init admin registr
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
app.UseExceptionHandler();
app.Run();


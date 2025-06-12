using Microsoft.EntityFrameworkCore;
using UserManagment;
using UserManagment.ApiContracts.User;
using UserManagment.Repositories;
using UserManagment.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICurrentActorService, CurrentActorService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite("Data Source=user.db:memory"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/openapi/v1.json", "User managment API"));
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
app.MapControllers();
app.Run();


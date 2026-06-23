using System.Reflection;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using BE.Repositories.Interfaces;
using BE.Repositories.Implementations;
using BE.Services.Interfaces;
using BE.Services.Implementations;
using BE.Middleware;
using StackExchange.Redis;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var redisConnection = builder.Configuration.GetConnectionString("RedisConnection");
if (string.IsNullOrEmpty(redisConnection))
{
    throw new Exception("Redis connection string is not configured.");
}
var redisConfig = StackExchange.Redis.ConfigurationOptions.Parse(redisConnection);
redisConfig.AbortOnConnectFail = false;
var redis = ConnectionMultiplexer.Connect(redisConfig);
var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add dbcontext to the services container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(ConnectionString, new MySqlServerVersion(new Version(8, 0, 44))));

builder.Services.AddControllers();

// Validator
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddSingleton<IConnectionMultiplexer>(redis);

// Scoped repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

// Scoped services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRegistrationFilterService, RegistrationFilterService>();
builder.Services.AddScoped<IJwtService, JwtService>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
app.Run();
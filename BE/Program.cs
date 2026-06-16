using System.Reflection;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using BE.Repositories.Interfaces;
using BE.Repositories.Implementations;
using BE.Services.Interfaces;
using BE.Services.Implementations;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
var redisConnection = builder.Configuration.GetConnectionString("RedisConnection");
if (string.IsNullOrEmpty(redisConnection))
{
    throw new Exception("Redis connection string is not configured.");
}
var redis = ConnectionMultiplexer.Connect(redisConnection);
var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add dbcontext to the services container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(ConnectionString, new MySqlServerVersion(new Version(8, 0, 44))));

builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddSingleton<IConnectionMultiplexer>(redis);

// Scoped repositories
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

// Scoped services
builder.Services.AddScoped<IRegistrationFilterService, RegistrationFilterService>();
builder.Services.AddScoped<IJwtService, JwtService>();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();

app.Run();
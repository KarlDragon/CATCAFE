using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using BE.Repositories.Interfaces;
using BE.Repositories.Implementations;
using BE.Services.Interfaces;
using BE.Services.Implementations;
using BE.Middleware;
using BE.Infrastructure.Queue;
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

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];
if (string.IsNullOrEmpty(secretKey))
{
    throw new Exception("JWT SecretKey is not configured.");
}
var signingKey = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(signingKey),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>{
                var authRepository = context.HttpContext.RequestServices.GetRequiredService<IAuthRepository>();
                var userIdClaim = context.Principal?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    context.Fail("Invalid token: User ID claim is missing or invalid.");
                    return;
                }

                var user = await authRepository.GetUserByIdAsync(userId);
                if (user == null)
                {
                    context.Fail("Unauthorized: User not found.");
                }
            }
        };
    });

builder.Services.AddAuthorization();

// Validator
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddSingleton<IConnectionMultiplexer>(redis);

// Scoped repositories
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<ICatRepository, CatRepository>();
builder.Services.AddScoped<IFoodDrinkRepository, FoodDrinkRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<ITableRepository, TableRepository>();

// Scoped services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRegistrationFilterService, RegistrationFilterService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ICatService, CatService>();
builder.Services.AddScoped<IFoodDrinkService, FoodDrinkService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<ITableService, TableService>();

// Queue and hosted worker
builder.Services.AddSingleton<IRequestQueue<BookingQueueRequest>>(_ => new RequestQueue<BookingQueueRequest>(capacity: 100));
builder.Services.AddHostedService<BookingQueueWorker>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

// Expose Program for integration testing
public partial class Program { }
using Microsoft.EntityFrameworkCore;
using BE.Repositories.Interfaces;
using BE.Repositories.Implementations;
var builder = WebApplication.CreateBuilder(args);

var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add dbcontext to the services container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(ConnectionString, new MySqlServerVersion(new Version(8, 0, 44))));

builder.Services.AddControllers();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();

app.Run();
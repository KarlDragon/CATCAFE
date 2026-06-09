using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add dbcontext to the services container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(ConnectionString, new MySqlServerVersion(new Version(8, 0, 44))));

builder.Services.AddControllers();


var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();

app.Run();
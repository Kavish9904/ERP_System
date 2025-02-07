using EmployeeRegisterAPI.Middlewares;
using EmployeeRegisterAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using TechAssignment.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Configure SQL Server connection
builder.Services.AddDbContext<EmployeeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection")));

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

// Register SignalR (Auto-registers IHubContext<EmployeeHub>)
builder.Services.AddSignalR();

var app = builder.Build();
var env = app.Services.GetRequiredService<IWebHostEnvironment>();
if (string.IsNullOrEmpty(env.WebRootPath))
{
    env.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
    Directory.CreateDirectory(env.WebRootPath); // Ensure the directory exists
}

// Middleware pipeline
app.UseMiddleware<ExceptionMiddleware>(); // Custom global exception handling middleware
app.UseCors("AllowAllOrigins");

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

// Configure SignalR Hub
app.MapHub<EmployeeHub>("/employeehub");

// Force .NET to listen on port 80
// app.Urls.Add("http://localhost:5000"); // Change to a specific port

// Start the application
app.Run();

using MicroserviceTemplate.Application;
using MicroserviceTemplate.Infrastructure;
using MicroserviceTemplate.Infrastructure.Persistence;
using Serilog;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
try
{
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();
}
catch
{
    // Fallback to basic console logging if configuration fails
    Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .CreateLogger();
}

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Microservice Template API",
        Version = "v1",
        Description = "A scalable microservice template with DDD and Clean Architecture"
    });
});

// Add Application and Infrastructure layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<MicroserviceTemplate.API.Middleware.ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Ensure database is created
try
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        // Try to connect to the database
        if (context.Database.CanConnect())
        {
            context.Database.EnsureCreated();
            Log.Information("Database connection successful and schema ensured.");
        }
        else
        {
            Log.Warning("Cannot connect to the database. The application will continue but database operations may fail.");
        }
    }
}
catch (Exception ex)
{
    Log.Error(ex, "An error occurred while connecting to the database. The application will continue but database operations may fail.");
    // Don't throw - allow the application to start even if database is not available
}

app.Run();


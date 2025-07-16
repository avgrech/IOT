using HomeAuthomationAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HomeAuthomationAPI.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Home Automation API", Version = "v1" });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine(">>> EF is using connection string: " + connectionString);

builder.Services.AddDbContext<HomeAutomationContext>(options =>
    options.UseSqlServer(connectionString)
           .EnableSensitiveDataLogging()
           .LogTo(Console.WriteLine, LogLevel.Information));


var app = builder.Build();

// Seed the database with default data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<HomeAutomationContext>();
    SeedData.Initialize(context);
}

// Enable Swagger middleware so that OpenAPI documentation is available
// in all environments.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Home Automation API v1");
});

app.UseHttpsRedirection();


app.MapControllers();

app.Run();

using HomeAuthomationAPI;
using HomeAuthomationAPI.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

public class ApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<HomeAutomationContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            var contextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(HomeAutomationContext));
            if (contextDescriptor != null)
            {
                services.Remove(contextDescriptor);
            }
            services.AddDbContext<HomeAutomationContext>(options =>
                options.UseInMemoryDatabase("TestDb"));

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<HomeAutomationContext>();
            SeedData.Initialize(db);
        });
    }

}

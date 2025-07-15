using HomeAuthomationAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace HomeAuthomationAPI.Data;

public static class SeedData
{
    public static void Initialize(HomeAutomationContext context)
    {
        // Ensure database is created
        context.Database.EnsureCreated();

        // Create default organisation if none exist
        if (!context.Organisations.Any())
        {
            context.Organisations.Add(new Organisation { Name = "Default Organisation" });
            context.SaveChanges();
        }

        // Create admin user if no users exist
        if (!context.Users.Any())
        {
            var organisationId = context.Organisations.First().Id;
            var admin = new User { Username = "admin", OrganisationId = organisationId };
            var hasher = new PasswordHasher<User>();
            admin.PasswordHash = hasher.HashPassword(admin, "admin");
            context.Users.Add(admin);
            context.SaveChanges();
        }
    }
}

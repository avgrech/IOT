using HomeAuthomationAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeAuthomationAPI.Data
{
    public class HomeAutomationContext : DbContext
    {
        public HomeAutomationContext(DbContextOptions<HomeAutomationContext> options) : base(options)
        {
        }

        public DbSet<Organisation> Organisations => Set<Organisation>();
        public DbSet<Property> Properties => Set<Property>();
        public DbSet<Configuration> Configurations => Set<Configuration>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Property>()
                .HasOne(p => p.Configuration)
                .WithOne(c => c.Property!)
                .HasForeignKey<Configuration>(c => c.PropertyId);
        }
    }
}

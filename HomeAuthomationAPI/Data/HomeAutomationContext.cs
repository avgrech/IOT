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
        public DbSet<DeviceStatus> DeviceStatuses => Set<DeviceStatus>();
        public DbSet<RouterDevice> RouterDevices => Set<RouterDevice>();
        public DbSet<Device> Devices => Set<Device>();
        public DbSet<Parameter> Parameters => Set<Parameter>();
        public DbSet<DeviceType> DeviceTypes => Set<DeviceType>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Property>()
                .HasOne(p => p.Configuration)
                .WithOne(c => c.Property!)
                .HasForeignKey<Configuration>(c => c.PropertyId);

            modelBuilder.Entity<Property>()
                .HasMany(p => p.RouterDevices)
                .WithOne(r => r.Property)
                .HasForeignKey(r => r.PropertyId);

            modelBuilder.Entity<RouterDevice>()
                .HasIndex(r => r.UniqueId)
                .IsUnique();

            modelBuilder.Entity<RouterDevice>()
                .HasMany(r => r.Devices)
                .WithOne(d => d.RouterDevice)
                .HasForeignKey(d => d.RouterDeviceId);

            modelBuilder.Entity<DeviceType>()
                .HasMany(dt => dt.Devices)
                .WithOne(d => d.DeviceType)
                .HasForeignKey(d => d.DeviceTypeId);

            modelBuilder.Entity<DeviceType>()
                .HasMany(dt => dt.Parameters)
                .WithOne(p => p.DeviceType)
                .HasForeignKey(p => p.DeviceTypeId);

            modelBuilder.Entity<Device>()
                .HasMany(d => d.Configurations)
                .WithOne(c => c.Device)
                .HasForeignKey(c => c.DeviceId);

            modelBuilder.Entity<RouterDevice>()
                .HasMany(r => r.Configurations)
                .WithOne(c => c.RouterDevice)
                .HasForeignKey(c => c.RouterDeviceId);

            modelBuilder.Entity<DeviceStatus>()
                 .HasOne(ds => ds.Device)
        .WithMany(d => d.DeviceStatuses) 
        .HasForeignKey(ds => ds.DeviceId)
        .OnDelete(DeleteBehavior.Cascade); ;
            
            

        }
    }
}

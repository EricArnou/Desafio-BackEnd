using Microsoft.EntityFrameworkCore;
using moto_rent.Features.Motors;
using moto_rent.Features.Riders;
using moto_rent.Features.Rentals;

namespace moto_rent.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Motor> Motors { get; set; }
    public DbSet<Rider> Riders { get; set; }
    public DbSet<Rental> Rentals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Motor
        modelBuilder.Entity<Motor>()
            .HasKey(m => m.Id);

        modelBuilder.Entity<Motor>()
            .Property(m => m.LicensePlate)
            .IsRequired();

        // Rider
        modelBuilder.Entity<Rider>()
            .HasKey(r => r.Id);

        modelBuilder.Entity<Rider>()
            .Property(r => r.Cnpj)
            .IsRequired();

        // Rental
        modelBuilder.Entity<Rental>()
            .HasKey(r => r.Id);

        modelBuilder.Entity<Rental>()
            .HasOne(r => r.Rider)
            .WithMany(r => r.Rentals)
            .HasForeignKey(r => r.RiderId);

        modelBuilder.Entity<Rental>()
            .HasOne(r => r.Motor)
            .WithMany(m => m.Rentals)
            .HasForeignKey(r => r.MotorId);
    }
}
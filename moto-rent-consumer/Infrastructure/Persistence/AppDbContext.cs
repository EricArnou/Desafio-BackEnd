using Microsoft.EntityFrameworkCore;
using moto_rent_consumer.Features.Motors;

namespace moto_rent_consumer.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Motor> Motors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Motor
        modelBuilder.Entity<Motor>()
            .HasKey(m => m.Id);

        modelBuilder.Entity<Motor>()
            .Property(m => m.LicensePlate)
            .IsRequired();
    }
}
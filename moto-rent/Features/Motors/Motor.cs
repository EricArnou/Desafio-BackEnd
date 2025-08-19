namespace moto_rent.Features.Motors;

using moto_rent.Features.Rentals;
using Microsoft.EntityFrameworkCore;

[Index(nameof(LicensePlate), IsUnique = true)]

public class Motor
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public int Year { get; set; }
    public string Model { get; set; } = string.Empty;
    public string LicensePlate { get; set; } = string.Empty;
    public bool IsAvailable { get; set; } = true;

    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}
namespace moto_rent.Features.Motors;
using moto_rent.Features.Rentals;

public class Motor
{
    public int Id { get; set; }
    public int Year { get; set; }
    public string Model { get; set; } = string.Empty;
    public string LicensePlate { get; set; } = string.Empty;
    public bool IsAvailable { get; set; } = true;

    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}
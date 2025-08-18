namespace moto_rent.Features.Motors;

public class Motor
{
    public int Id { get; set; }
    public int Year { get; set; }
    public string Model { get; set; } = string.Empty;
    public string LicensePlate { get; set; } = string.Empty;
    public bool IsAvailable { get; set; } = true;
}
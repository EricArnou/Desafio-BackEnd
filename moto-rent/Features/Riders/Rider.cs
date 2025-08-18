namespace moto_rent.Features.Riders;
using moto_rent.Features.Rentals;
public class Rider
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Cnh { get; set; } = string.Empty;

    public enum CnhCategory
    {
        A,
        B,
        AB
    }
    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}
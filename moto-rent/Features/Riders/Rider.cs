using Microsoft.EntityFrameworkCore;
using moto_rent.Features.Rentals;

namespace moto_rent.Features.Riders;

[Index(nameof(Cnpj), IsUnique = true)]
[Index(nameof(Cnh), IsUnique = true)]
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
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using moto_rent.Features.Rentals;

namespace moto_rent.Features.Riders;

[Index(nameof(Cnpj), IsUnique = true)]
[Index(nameof(Cnh), IsUnique = true)]
public class Rider
{
    [Key]
    public string Id { get; private set; } = Guid.NewGuid().ToString("N");
    public string Name { get; private set; } = string.Empty;
    public string Cnpj { get; private set; } = string.Empty;
    public DateTime BirthDate { get; private set; }
    public string Cnh { get; private set; } = string.Empty;

    public enum CnhCategory
    {
        A,
        B,
        AB
    }

    public string ImageCnh { get; private set; } = string.Empty;
    public ICollection<Rental> Rentals { get; private set; } = new List<Rental>();
}
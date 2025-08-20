using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using moto_rent.Features.Rentals;
using moto_rent.Features.Riders.DTOs;

namespace moto_rent.Features.Riders;

[Index(nameof(Cnpj), IsUnique = true)]
[Index(nameof(Cnh), IsUnique = true)]
public class Rider
{

    public Rider() { }

    [Key]
    public string Id { get; private set; } = Guid.NewGuid().ToString("N");
    public string Name { get; private set; } = string.Empty;
    public string Cnpj { get; private set; } = string.Empty;
    public DateTime BirthDate { get; private set; }
    public string Cnh { get; private set; } = string.Empty;

    public CnhCategory CnhCategory { get; private set; }

    public string ImageCnh { get; private set; } = string.Empty;
    public ICollection<Rental> Rentals { get; private set; } = new List<Rental>();

    public static Rider FromDto(RiderDto dto)
    {
        return new Rider
        {
            Id = dto.identificador,
            Name = dto.nome,
            Cnpj = dto.cnpj,
            BirthDate = dto.dataNascimento,
            Cnh = dto.cnh,
            CnhCategory = Enum.Parse<CnhCategory>(dto.CnhCategory),
            ImageCnh = dto.imagemCnh
        };
    }
}
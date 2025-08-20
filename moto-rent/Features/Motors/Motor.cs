namespace moto_rent.Features.Motors;

using moto_rent.Features.Rentals;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using moto_rent.Features.Motors.DTOs;

[Index(nameof(LicensePlate), IsUnique = true)]

public class Motor
{

    public Motor() {}

    [Key]
    public string Id { get; private set; } = Guid.NewGuid().ToString("N");
    public int Year { get; private set; }
    public string Model { get; private set; } = string.Empty;
    public string LicensePlate { get; private set; } = string.Empty;
    public bool IsAvailable { get; private set; } = true;

    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();

    public static Motor FromDto(MotorDto motor)
    {
        return new Motor
        {
            Id = motor.identificador,
            Year = motor.ano,
            Model = motor.modelo,
            LicensePlate = motor.placa
        };
    }

    public void SetLicensePlate(string newLicensePlate)
    {
        this.LicensePlate = newLicensePlate;
    }
}
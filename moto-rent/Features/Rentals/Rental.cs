using moto_rent.Features.Motors;
using moto_rent.Features.Riders;
using moto_rent.Features.Riders.DTOs;

namespace moto_rent.Features.Rentals;

public class Rental
{
    public string Id { get; private set; } = Guid.NewGuid().ToString("N");

    public string RiderId { get; set; } = null!;
    public Rider Rider { get; set; } = null!;

    public string MotorId { get; set; } = null!;
    public Motor Motor { get; set; } = null!;

    public DateTime StartRentalDate { get; set; }
    public DateTime EndRentalDate { get; set; }
    public DateTime ExpectedRentalEndDate { get; set; }
    public DateTime RentalReturnDate { get; set; }
    public RentalPlans RentalPlan { get; set; }
    public decimal TotalPrice { get; set; }

    public static Rental FromDto(CreateRentalDto dto)
    {
        return new Rental
        {
            RiderId = dto.entregador_id,
            MotorId = dto.moto_id,
            StartRentalDate = dto.data_inicio,
            EndRentalDate = dto.data_termino,
            ExpectedRentalEndDate = dto.data_previsao_termino,
            RentalPlan = (RentalPlans)dto.plano
        };
    }
}
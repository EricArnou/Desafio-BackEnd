using moto_rent.Features.Motors;
using moto_rent.Features.Riders;

namespace moto_rent.Features.Rentals;

public class Rental
{
    public int Id { get; set; }

    public string RiderId { get; set; } = null!;
    public Rider Rider { get; set; } = null!;

    public string MotorId { get; set; } = null!;
    public Motor Motor { get; set; } = null!;

    public DateTime StartRentalDate { get; set; }
    public DateTime EndRentalDate { get; set; }
    public DateTime ExpectedRentalEndDate { get; set; }
    public RentalPlans RentalPlan { get; set; }
    public decimal TotalPrice { get; set; }
}
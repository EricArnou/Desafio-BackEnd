using moto_rent.Features.Motors.DTOs;
using moto_rent.Features.Rentals;

namespace moto_rent.Features.Riders.DTOs
{
    public class CreateRentalDto
    {

        public CreateRentalDto() { }
        public string entregador_id { get; set; } = null!;
        public string moto_id { get; set; } = null!;
        public DateTime data_inicio { get; set; }
        public DateTime data_termino { get; set; }
        public DateTime data_prevista_termino { get; set; }
        public int plano { get; set; } = 0;
    }
    public class RentalDto
    {

        public RentalDto(Rental rental)
        {
            identificador = rental.Id;
            entregador = new RiderDto(rental.Rider);
            moto = new MotorDto(rental.Motor);
            data_inicio = rental.StartRentalDate;
            data_termino = rental.EndRentalDate;
            data_prevista_termino = rental.ExpectedRentalEndDate;
            plano = rental.RentalPlan.ToString();
            preco_total = rental.TotalPrice;
        }

        public RentalDto() { }
        public string identificador { get; set; } = Guid.NewGuid().ToString("N");
        public RiderDto entregador { get; set; } = null!;
        public MotorDto moto { get; set; } = null!;
        public DateTime data_inicio { get; set; }
        public DateTime data_termino { get; set; }
        public DateTime data_prevista_termino { get; set; }
        public string plano { get; set; } = string.Empty;
        public decimal preco_total { get; set; }
    }

    public class UpdateRentalDto
    {
        public DateTime? data_termino { get; set; }
    }

}

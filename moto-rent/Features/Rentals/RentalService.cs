

using moto_rent.Features.Motors;
using moto_rent.Features.Riders;
using moto_rent.Features.Riders.DTOs;

namespace moto_rent.Features.Rentals.Services
{
    public class RentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IMotorRepository _motorRepository;
        private readonly IRiderRepository _riderRepository;

        public RentalService(IRentalRepository rentalRepository, IMotorRepository motorRepository, IRiderRepository riderRepository)
        {
            _rentalRepository = rentalRepository;
            _motorRepository = motorRepository;
            _riderRepository = riderRepository;
        }

        public async Task<Rental?> GetRentalByIdAsync(string id)
        {
            return await _rentalRepository.GetRentalByIdAsync(id);
        }

        public async Task<RentalDto> CreateRentalAsync(CreateRentalDto rentalDto)
        {
            var rental = Rental.FromDto(rentalDto);

            var motor = await _motorRepository.GetMotorByIdAsync(rentalDto.moto_id);

            if (motor == null)
            {
                throw new ArgumentException("Motor not found for the given moto_id.");
            }

            rental.Motor = motor;

            var rider = await _riderRepository.GetRiderByIdAsync(rentalDto.entregador_id);

            if (rider == null)
            {
                throw new ArgumentException("Rider not found for the given entregador_id.");
            }

            rental.Rider = rider;

            await _rentalRepository.CreateRentalAsync(rental);
            return new RentalDto(rental);
        }

        public async Task<RentalDto?> UpdateRentalAsync(string id, UpdateRentalDto rental)
        {
            var existing = await _rentalRepository.GetRentalByIdAsync(id);

            if (existing == null)
            {
                throw new ArgumentException("Rental not found");
            }

            if (rental.data_termino.HasValue)
            {
                existing.EndRentalDate = rental.data_termino.Value;
            }

            await _rentalRepository.UpdateRentalAsync(existing);

            return new RentalDto(existing);
        }

        public decimal CalculateRentalPrice(Rental rental)
        {

            decimal price = 0;

            switch (rental.RentalPlan)
                {
                    case RentalPlans.Weekly:
                        price = 30 * (decimal)(rental.EndRentalDate - rental.StartRentalDate.AddDays(1)).TotalDays;
                        break;
                    case RentalPlans.BiWeekly:
                        price = 28 * (decimal)(rental.EndRentalDate - rental.StartRentalDate.AddDays(1)).TotalDays;
                        break;
                    case RentalPlans.Monthly:
                        price = 22 * (decimal)(rental.EndRentalDate - rental.StartRentalDate.AddDays(1)).TotalDays;
                        break;
                    case RentalPlans.Fortnightly:
                        price = 20 * (decimal)(rental.EndRentalDate - rental.StartRentalDate.AddDays(1)).TotalDays;
                        break;
                    case RentalPlans.Fifty:
                        price = 18 * (decimal)(rental.EndRentalDate - rental.StartRentalDate.AddDays(1)).TotalDays;
                        break;
                }

            if (rental.EndRentalDate < rental.ExpectedRentalEndDate)
            {
                if (rental.RentalPlan == RentalPlans.Weekly)
                    price += (decimal)(0.2 * ((rental.ExpectedRentalEndDate - rental.EndRentalDate).TotalDays * 30));

                if (rental.RentalPlan == RentalPlans.BiWeekly)
                    price += (decimal)(0.4 * ((rental.ExpectedRentalEndDate - rental.EndRentalDate).TotalDays * 28));
            }
            else if (rental.EndRentalDate > rental.ExpectedRentalEndDate)
            {
                price += 50 * (decimal)(rental.EndRentalDate - rental.ExpectedRentalEndDate).TotalDays;
            }

            return price;
        }
    }
}

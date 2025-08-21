

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
        private const decimal WeeklyRentalValue = 30;
        private const decimal BiWeeklyRentalValue = 28;
        private const decimal MonthlyRentalValue = 22;
        private const decimal FortnightlyRentalValue = 20;
        private const decimal FiftyRentalValue = 18;

        public RentalService(IRentalRepository rentalRepository, IMotorRepository motorRepository, IRiderRepository riderRepository)
        {
            _rentalRepository = rentalRepository;
            _motorRepository = motorRepository;
            _riderRepository = riderRepository;
        }

        public async Task<GetRentalDto> GetRentalByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("id is required");
            }

            var rental = await _rentalRepository.GetRentalByIdAsync(id);
            
            if (rental == null)
            {
                throw new KeyNotFoundException("id not found");
            }

            return new GetRentalDto(rental);
        }

        public async Task<RentalDto> CreateRentalAsync(CreateRentalDto rentalDto)
        {
            var rental = Rental.FromDto(rentalDto);

            var motor = await _motorRepository.GetMotorByIdAsync(rentalDto.moto_id);

            if (motor == null)
            {
                throw new ArgumentException("Motor not found for the given moto_id.");
            }

            if(motor.IsAvailable == false)
            {
                throw new ArgumentException("Motor is not available for rental.");
            }

            rental.Motor = motor;

            var rider = await _riderRepository.GetRiderByIdAsync(rentalDto.entregador_id);

            if (rider == null)
            {
                throw new ArgumentException("Rider not found for the given entregador_id.");
            }

            if(rider.CnhCategory != CnhCategory.A)
            {
                throw new ArgumentException("Rider is not allowed to rent this motor.");
            }

            rental.Rider = rider;

            await _rentalRepository.CreateRentalAsync(rental);

            rental.Motor.SetAvailability(false);
            return new RentalDto(rental);
        }

        public async Task UpdateRentalAsync(string id, UpdateRentalDto rental)
        {
            var existing = await _rentalRepository.GetRentalByIdAsync(id);

            if (existing == null)
            {
                throw new ArgumentException("Rental not found");
            }

            if (rental.data_devolucao.HasValue)
            {
                existing.RentalReturnDate = (DateTime)rental.data_devolucao;
            }

            existing.TotalPrice = CalculateRentalPrice(existing);
            existing.Motor.SetAvailability(true);
            await _rentalRepository.UpdateRentalAsync(existing);
        }

        public decimal CalculateRentalPrice(Rental rental)
        {

            decimal price = 0;

            if (rental.RentalReturnDate.Date >= rental.ExpectedRentalEndDate.Date)
            {

                switch (rental.RentalPlan)
                {
                    case RentalPlans.Weekly:
                        price = WeeklyRentalValue * (decimal)RentalPlans.Weekly;
                        break;
                    case RentalPlans.BiWeekly:
                        price = BiWeeklyRentalValue * (decimal)RentalPlans.BiWeekly;
                        break;
                    case RentalPlans.Monthly:
                        price = MonthlyRentalValue * (decimal)RentalPlans.Monthly;
                        break;
                    case RentalPlans.Fortnightly:
                        price = FortnightlyRentalValue * (decimal)RentalPlans.Fortnightly;
                        break;
                    case RentalPlans.Fifty:
                        price = FiftyRentalValue * (decimal)RentalPlans.Fifty;
                        break;
                }
            }

            if (rental.RentalReturnDate.Date < rental.ExpectedRentalEndDate.Date)
            {
                var rentedDays = Math.Max(1, (rental.RentalReturnDate.Date - rental.StartRentalDate.Date).Days + 1);
                var remainingDays = Math.Max(0, (rental.ExpectedRentalEndDate.Date - rental.RentalReturnDate.Date).Days);

                switch (rental.RentalPlan)
                {
                    case RentalPlans.Weekly:
                        price = (WeeklyRentalValue * rentedDays) + (WeeklyRentalValue * 0.2m * remainingDays);
                        break;
                    case RentalPlans.BiWeekly:
                        price = (BiWeeklyRentalValue * rentedDays) + (BiWeeklyRentalValue * 0.4m * remainingDays);
                        break;
                }
            }
            else if (rental.RentalReturnDate.Date > rental.ExpectedRentalEndDate.Date)
            {
                var additionalRentedDays = (rental.RentalReturnDate.Date - rental.ExpectedRentalEndDate.Date).TotalDays;
                price += 50 * (decimal)additionalRentedDays;
            }

            return price;
        }
    }
}

using moto_rent_consumer.Features.Motors;
using moto_rent_consumer.Features.Motors.DTOs;

namespace moto_rent_consumer.Services
{
    public class MotorService
    {
        private readonly IMotorRepository _repository;

        public MotorService(IMotorRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateMotorAsync(MotorDto motor)
        {
            if (string.IsNullOrWhiteSpace(motor.placa))
                throw new ArgumentException("License plate is required");

            if (await _repository.GetLicensePlateAsync(motor.placa))
                throw new ArgumentException("License plate already exists");

            var motorEntity = Motor.FromDto(motor);
            await _repository.AddMotorAsync(motorEntity);
        }
    }
}

// Services/MotorService.cs
using moto_rent.Features.Motors;

namespace moto_rent.Services
{
    public class MotorService
    {
        private readonly MotorRepository _repository;

        public MotorService(MotorRepository repository)
        {
            _repository = repository;
        }

        public async Task<Motor?> GetMotorByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Id is required");

            var motor = await _repository.GetMotorByIdAsync(id);

            if (motor == null)
                throw new KeyNotFoundException("id not found");
                
            return motor;
        }

        public async Task<List<Motor>> GetAllMotorsAsync()
        {
            return await _repository.GetAllMotorsAsync();
        }

        public async Task CreateMotorAsync(Motor motor)
        {
            // validações de negócio
            if (string.IsNullOrWhiteSpace(motor.LicensePlate))
                throw new ArgumentException("License plate is required");

            await _repository.AddMotorAsync(motor);
        }

        public async Task UpdateMotorAsync(string id, string newLicensePlate)
        {

            if(id == null)
                throw new ArgumentException("Id is required");

            if (string.IsNullOrWhiteSpace(newLicensePlate))
                throw new ArgumentException("New license plate is required");

            var motor = await _repository.GetMotorByIdAsync(id);

            if (motor == null)
                throw new KeyNotFoundException("Id not found");

            if (await _repository.GetLicensePlateAsync(newLicensePlate))
                throw new ArgumentException("License plate already exists");

            motor.LicensePlate = newLicensePlate;
            await _repository.UpdateMotorAsync(motor);
        }

        public async Task DeleteMotorAsync(string id)
        {

            if(id == null)
                throw new ArgumentException("Id is required");

            var motor = await _repository.GetMotorByIdAsync(id);

            if (motor == null)
                throw new ArgumentException("Id not found");

            if (motor == null) throw new KeyNotFoundException("Motor not found");

            await _repository.DeleteMotorAsync(motor);
        }
    }
}

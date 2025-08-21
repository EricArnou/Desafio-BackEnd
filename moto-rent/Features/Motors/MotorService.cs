using moto_rent.Features.Motors;
using moto_rent.Features.Motors.DTOs;

namespace moto_rent.Services
{
    public class MotorService
    {
        private readonly IMotorRepository _repository;
        private readonly MotoEventPublisher _eventPublisher;
        private readonly ILogger<MotorService> _logger;


        public MotorService(IMotorRepository repository, MotoEventPublisher eventPublisher, ILogger<MotorService> logger)
        {
            _repository = repository;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        public async Task<MotorDto?> GetMotorByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogError("Invalid motor id: {Id}", id);
                throw new ArgumentException("Id is required");
            }

            var motor = await _repository.GetMotorByIdAsync(id);

            if (motor == null)
            {
                _logger.LogError("Motor not found: {Id}", id);
                throw new KeyNotFoundException("id not found");
            }

            return new MotorDto(motor);
        }

        public async Task<List<MotorDto>> GetAllMotorsAsync()
        {
            var motors = await _repository.GetAllMotorsAsync();
            return motors.Select(m => new MotorDto(m)).ToList();
        }

        public async Task CreateMotorAsync(MotorDto motor)
        {
            if (string.IsNullOrWhiteSpace(motor.placa))
            {
                _logger.LogError("Invalid license plate: {Placa}", motor.placa);
                throw new ArgumentException("License plate is required");
            }

            if (await _repository.GetLicensePlateAsync(motor.placa))
            {
                _logger.LogError("License plate already exists: {Placa}", motor.placa);
                throw new ArgumentException("License plate already exists");
            }

            var motorEntity = Motor.FromDto(motor);
            await _repository.AddMotorAsync(motorEntity);
            _eventPublisher.PublishMotoCadastrada(motor);
            _logger.LogInformation("Motor with id {Id} created", motor.identificador);
        }

        public async Task UpdateMotorAsync(string id, string newLicensePlate)
        {

            if (id == null)
            {
                _logger.LogError("Invalid motor id: {Id}", id);
                throw new ArgumentException("Id is required");
            }

            if (string.IsNullOrWhiteSpace(newLicensePlate))
            {
                _logger.LogError("Invalid new license plate: {NewLicensePlate}", newLicensePlate);
                throw new ArgumentException("New license plate is required");
            }

            var motor = await _repository.GetMotorByIdAsync(id);

            if (motor == null)
            {
                _logger.LogError("Motor not found: {Id}", id);
                throw new KeyNotFoundException("Id not found");
            }

            if (await _repository.GetLicensePlateAsync(newLicensePlate))
            {
                _logger.LogError("License plate already exists: {Placa}", newLicensePlate);
                throw new ArgumentException("License plate already exists");
            }

            motor.SetLicensePlate(newLicensePlate);
            await _repository.UpdateMotorAsync(motor);
            _logger.LogInformation("Motor with id {Id} updated with new license plate {NewLicensePlate}", id, newLicensePlate);
        }

        public async Task DeleteMotorAsync(string id)
        {

            if (id == null)
            {
                _logger.LogError("Invalid motor id: {Id}", id);
                throw new ArgumentException("Id is required");
            }

            var motor = await _repository.GetMotorByIdAsync(id);

            if (motor == null)
            {
                _logger.LogError("Motor not found: {Id}", id);
                throw new ArgumentException("Id not found");
            }

            if (motor == null)
            {
                _logger.LogError("Motor not found: {Id}", id);
                throw new KeyNotFoundException("Motor not found");
            }

            await _repository.DeleteMotorAsync(motor);
            _logger.LogInformation("Motor with id {Id} deleted", id);
        }
    }
}

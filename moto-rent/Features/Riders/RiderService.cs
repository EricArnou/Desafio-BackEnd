using moto_rent.Features.Riders.DTOs;


namespace moto_rent.Features.Riders.Services
{
    public class RiderService
    {
        private readonly IRiderRepository _repository;
        private readonly ILogger<RiderService> _logger;

        public RiderService(IRiderRepository repository, ILogger<RiderService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task CreateRiderAsync(RiderDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.nome))
            {
                _logger.LogError("Invalid rider name: {Rider}", dto.nome);
                throw new ArgumentException("Name is required");
            }

            if (await _repository.CnpjExistsAsync(dto.cnpj))
            {
                _logger.LogError("CNPJ already exists: {Cnpj}", dto.cnpj);
                throw new ArgumentException("CNPJ already exists");
            }

            if (await _repository.CnhNumberExistsAsync(dto.numero_cnh))
            {
                _logger.LogError("CNH number already exists: {CnhNumber}", dto.numero_cnh);
                throw new ArgumentException("CNH number already exists");
            }

            if (!new[] { "A", "B", "AB" }.Contains(dto.tipo_cnh))
            {
                _logger.LogError("Invalid CNH type: {CnhType}", dto.tipo_cnh);
                throw new ArgumentException("Invalid CNH type. Must be A, B, or AB");
            }

            var rider = Rider.FromDto(dto);

            await _repository.AddRiderAsync(rider);
            _logger.LogInformation("Rider with id {Id} created", rider.Id);
        }
    }
}

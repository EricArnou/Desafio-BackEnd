using moto_rent.Features.Riders.DTOs;


namespace moto_rent.Features.Riders.Services
{
    public class RiderService
    {
        private readonly IRiderRepository _repository;

        public RiderService(IRiderRepository repository)
        {
            _repository = repository;
        }

        public async Task<RiderDto> CreateRiderAsync(RiderDto dto)
        {
            // Validações básicas
            if (string.IsNullOrWhiteSpace(dto.nome))
                throw new ArgumentException("Name is required");

            if (await _repository.CnpjExistsAsync(dto.cnpj))
                throw new ArgumentException("CNPJ already exists");

            if (await _repository.CnhNumberExistsAsync(dto.numero_cnh))
                throw new ArgumentException("CNH number already exists");

            if (!new[] { "A", "B", "AB" }.Contains(dto.tipo_cnh))
                throw new ArgumentException("Invalid CNH type. Must be A, B, or AB");

            var rider = Rider.FromDto(dto);

            await _repository.AddRiderAsync(rider);

            return new RiderDto(rider);
        }
    }
}

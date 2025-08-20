using moto_rent.Persistence;
using Microsoft.EntityFrameworkCore;

namespace moto_rent.Features.Riders
{
    public interface IRiderRepository
    {
        Task AddRiderAsync(Rider rider);
        Task<bool> CnpjExistsAsync(string cnpj);
        Task<bool> CnhNumberExistsAsync(string cnhNumber);
    }
    public class RiderRepository : IRiderRepository
    {
        private readonly AppDbContext _context;

        public RiderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddRiderAsync(Rider rider)
        {
            _context.Riders.Add(rider);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CnpjExistsAsync(string cnpj)
        {
            return await _context.Riders.AnyAsync(r => r.Cnpj == cnpj);
        }

        public async Task<bool> CnhNumberExistsAsync(string cnhNumber)
        {
            return await _context.Riders.AnyAsync(r => r.Cnh == cnhNumber);
        }
    }
}

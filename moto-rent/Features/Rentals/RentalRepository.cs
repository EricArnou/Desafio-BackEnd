using moto_rent.Persistence;

namespace moto_rent.Features.Rentals
{
    public interface IRentalRepository
    {
        Task<Rental?> GetRentalByIdAsync(string id);
        Task CreateRentalAsync(Rental rental);
        Task UpdateRentalAsync(Rental rental);
    }
    public class RentalRepository : IRentalRepository
    {
        private readonly AppDbContext _context;

        public RentalRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Rental?> GetRentalByIdAsync(string id)
        {
            return await _context.Rentals.FindAsync(id);
        }

        public async Task CreateRentalAsync(Rental rental)
        {
            await _context.Rentals.AddAsync(rental);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRentalAsync(Rental rental)
        {
            _context.Rentals.Update(rental);
            await _context.SaveChangesAsync();
        }
    }
}

using moto_rent_consumer.Persistence;
using Microsoft.EntityFrameworkCore;

namespace moto_rent_consumer.Features.Motors
{
    public interface IMotorRepository
    {
        Task AddMotorAsync(Motor motor);
        Task<bool> GetLicensePlateAsync(string licensePlate);

    }

    public class MotorRepository : IMotorRepository
    {
        private readonly AppDbContext _context;

        public MotorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddMotorAsync(Motor motor)
        {
            _context.Motors.Add(motor);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> GetLicensePlateAsync(string licensePlate)
        {
            return await _context.Motors.AnyAsync(m => m.LicensePlate == licensePlate);
        }
    }
}

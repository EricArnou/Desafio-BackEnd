// Data/Repositories/MotorRepository.cs
using moto_rent.Persistence;
using Microsoft.EntityFrameworkCore;

namespace moto_rent.Features.Motors
{
    public class MotorRepository
    {
        private readonly AppDbContext _context;

        public MotorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Motor?> GetMotorByIdAsync(Guid id)
        {
            return await _context.Motors.FindAsync(id);
        }

        public async Task<List<Motor>> GetAllMotorsAsync()
        {
            return await _context.Motors.ToListAsync();
        }

        public async Task AddMotorAsync(Motor motor)
        {
            _context.Motors.Add(motor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMotorAsync(Motor motor)
        {
            _context.Motors.Update(motor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMotorAsync(Motor motor)
        {
            _context.Motors.Remove(motor);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> GetLicensePlateAsync(string licensePlate)
        {
            return await _context.Motors.AnyAsync(m => m.LicensePlate == licensePlate);
        }
    }
}

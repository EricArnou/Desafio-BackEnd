// Data/Repositories/MotorRepository.cs
using moto_rent.Persistence;
using Microsoft.EntityFrameworkCore;
using moto_rent.Features.Motors; // Add this if IMotorRepository is defined in this namespace

namespace moto_rent.Features.Motors
{
    public interface IMotorRepository
    {
        Task<Motor?> GetMotorByIdAsync(string id);
        Task<List<Motor>> GetAllMotorsAsync();
        Task AddMotorAsync(Motor motor);
        Task UpdateMotorAsync(Motor motor);
        Task DeleteMotorAsync(Motor motor);
        Task<bool> GetLicensePlateAsync(string licensePlate);
    }

    public class MotorRepository : IMotorRepository
    {
        private readonly AppDbContext _context;

        public MotorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Motor?> GetMotorByIdAsync(string id)
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

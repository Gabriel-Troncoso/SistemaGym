using Microsoft.EntityFrameworkCore;
using SistemaGym.Core.Entities;
using SistemaGym.Core.Interfaces;
using SistemaGym.Infrastructure.Data;

namespace SistemaGym.Infrastructure.Repositories
{
    public class MembresiaRepository : IMembresiaRepository
    {
        private readonly SistemaGymContext _context;

        public MembresiaRepository(SistemaGymContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Membresia>> GetAllMembresiasAsync()
        {
            var membresias = await _context.Membresias
                .Include(x => x.Cliente)
                .Include(x => x.PlanMembresia)
                .ToListAsync();

            return membresias;
        }

        public async Task<Membresia> GetMembresiaByIdAsync(int id)
        {
            var membresia = await _context.Membresias
                .Include(x => x.Cliente)
                .Include(x => x.PlanMembresia)
                .FirstOrDefaultAsync(x => x.Id == id);

            return membresia;
        }

        public async Task InsertMembresia(Membresia membresia)
        {
            _context.Membresias.Add(membresia);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMembresia(Membresia membresia)
        {
            _context.Membresias.Update(membresia);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMembresia(Membresia membresia)
        {
            _context.Membresias.Remove(membresia);
            await _context.SaveChangesAsync();
        }
    }
}
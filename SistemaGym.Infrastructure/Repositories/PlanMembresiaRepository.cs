using Microsoft.EntityFrameworkCore;
using SistemaGym.Core.Entities;
using SistemaGym.Core.Interfaces;
using SistemaGym.Infrastructure.Data;

namespace SistemaGym.Infrastructure.Repositories
{
    public class PlanMembresiaRepository : IPlanMembresiaRepository
    {
        private readonly SistemaGymContext _context;

        public PlanMembresiaRepository(SistemaGymContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PlanMembresia>> GetAllPlanesAsync()
        {
            var planes = await _context.PlanesMembresia.ToListAsync();
            return planes;
        }

        public async Task<PlanMembresia> GetPlanByIdAsync(int id)
        {
            var plan = await _context.PlanesMembresia.FirstOrDefaultAsync(x => x.Id == id);
            return plan;
        }

        public async Task InsertPlan(PlanMembresia plan)
        {
            _context.PlanesMembresia.Add(plan);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePlan(PlanMembresia plan)
        {
            _context.PlanesMembresia.Update(plan);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePlan(PlanMembresia plan)
        {
            _context.PlanesMembresia.Remove(plan);
            await _context.SaveChangesAsync();
        }
    }
}
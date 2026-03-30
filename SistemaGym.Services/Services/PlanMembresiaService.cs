using SistemaGym.Core.Entities;
using SistemaGym.Core.Interfaces;
using SistemaGym.Services.Interfaces;

namespace SistemaGym.Services.Services
{
    public class PlanMembresiaService : IPlanMembresiaService
    {
        public readonly IBaseRepository<PlanMembresia> _planRepository;

        public PlanMembresiaService(IBaseRepository<PlanMembresia> planRepository)
        {
            _planRepository = planRepository;
        }

        public async Task<IEnumerable<PlanMembresia>> GetAllPlanesAsync()
        {
            return await _planRepository.GetAll();
        }

        public async Task<PlanMembresia> GetPlanByIdAsync(int id)
        {
            return await _planRepository.GetById(id);
        }

        public async Task InsertPlan(PlanMembresia plan)
        {
            if (string.IsNullOrWhiteSpace(plan.NombrePlan))
                throw new Exception("El nombre del plan es obligatorio");

            if (!plan.Precio.HasValue || plan.Precio <= 0)
                throw new Exception("El precio debe ser mayor a cero");

            if (!plan.DuracionDias.HasValue || plan.DuracionDias <= 0)
                throw new Exception("La duración debe ser mayor a cero");

            await _planRepository.Add(plan);
        }

        public async Task UpdatePlan(PlanMembresia plan)
        {
            await _planRepository.Update(plan);
        }

        public async Task DeletePlan(int id)
        {
            await _planRepository.Delete(id);
        }
    }
}
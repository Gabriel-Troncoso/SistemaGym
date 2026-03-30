using SistemaGym.Core.Entities;

namespace SistemaGym.Core.Interfaces
{
    public interface IPlanMembresiaRepository
    {
        Task<IEnumerable<PlanMembresia>> GetAllPlanesAsync();
        Task<PlanMembresia> GetPlanByIdAsync(int id);
        Task InsertPlan(PlanMembresia plan);
        Task UpdatePlan(PlanMembresia plan);
        Task DeletePlan(PlanMembresia plan);
    }
}
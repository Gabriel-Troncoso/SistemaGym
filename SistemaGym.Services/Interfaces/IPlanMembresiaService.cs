using SistemaGym.Core.Entities;

namespace SistemaGym.Services.Interfaces
{
    public interface IPlanMembresiaService
    {
        Task<IEnumerable<PlanMembresia>> GetAllPlanesAsync();
        Task<PlanMembresia> GetPlanByIdAsync(int id);
        Task InsertPlan(PlanMembresia plan);
        Task UpdatePlan(PlanMembresia plan);
        Task DeletePlan(int id);
    }
}
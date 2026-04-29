using SistemaGym.Core.Entities;
using SistemaGym.Core.QueryFilters;

namespace SistemaGym.Services.Interfaces
{
    public interface IPlanMembresiaService
    {
        Task<IEnumerable<PlanMembresia>> GetAllPlanesAsync(
            PlanMembresiaQueryFilter? filters = null);

        Task<IEnumerable<PlanMembresia>> GetAllPlanesDapperAsync(
            int limit = 10);

        Task<PlanMembresia> GetPlanByIdAsync(int id);

        Task InsertPlan(PlanMembresia plan);

        void UpdatePlan(PlanMembresia plan);

        Task DeletePlan(int id);
    }
}
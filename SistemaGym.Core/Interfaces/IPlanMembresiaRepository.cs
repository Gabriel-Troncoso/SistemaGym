using SistemaGym.Core.Entities;

namespace SistemaGym.Core.Interfaces
{
    public interface IPlanMembresiaRepository : IBaseRepository<PlanMembresia>
    {
        Task<IEnumerable<PlanMembresia>> GetAllPlanesDapperAsync(int limit = 10);
    }
}
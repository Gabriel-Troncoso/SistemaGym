using SistemaGym.Core.Entities;

namespace SistemaGym.Core.Interfaces
{
    public interface IMembresiaRepository : IBaseRepository<Membresia>
    {
        Task<IEnumerable<Membresia>> GetAllMembresiasDapperAsync(int limit = 10);
    }
}
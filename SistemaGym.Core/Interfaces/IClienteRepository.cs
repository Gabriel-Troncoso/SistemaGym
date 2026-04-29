using SistemaGym.Core.Entities;

namespace SistemaGym.Core.Interfaces
{
    public interface IClienteRepository : IBaseRepository<Cliente>
    {
        Task<IEnumerable<Cliente>> GetAllClientesDapperAsync(int limit = 10);
    }
}
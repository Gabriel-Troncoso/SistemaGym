using SistemaGym.Core.Entities;
using SistemaGym.Core.QueryFilters;

namespace SistemaGym.Services.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> GetAllClientesAsync(ClienteQueryFilter? filters = null);

        Task<IEnumerable<Cliente>> GetAllClientesDapperAsync(int limit = 10);

        Task<Cliente> GetClienteByIdAsync(int id);

        Task InsertCliente(Cliente cliente);

        void UpdateCliente(Cliente cliente);

        Task DeleteCliente(int id);
    }
}
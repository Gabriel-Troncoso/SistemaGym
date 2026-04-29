using SistemaGym.Core.Entities;
using SistemaGym.Core.QueryFilters;

namespace SistemaGym.Services.Interfaces
{
    public interface IMembresiaService
    {
        Task<IEnumerable<Membresia>> GetAllMembresiasAsync(
            MembresiaQueryFilter? filters = null);

        Task<IEnumerable<Membresia>> GetAllMembresiasDapperAsync(
            int limit = 10);

        Task<Membresia> GetMembresiaByIdAsync(int id);

        Task InsertMembresia(Membresia membresia);

        void UpdateMembresia(Membresia membresia);

        Task DeleteMembresia(int id);
    }
}
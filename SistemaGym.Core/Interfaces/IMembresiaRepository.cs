using SistemaGym.Core.Entities;

namespace SistemaGym.Core.Interfaces
{
    public interface IMembresiaRepository
    {
        Task<IEnumerable<Membresia>> GetAllMembresiasAsync();
        Task<Membresia> GetMembresiaByIdAsync(int id);
        Task InsertMembresia(Membresia membresia);
        Task UpdateMembresia(Membresia membresia);
        Task DeleteMembresia(Membresia membresia);
    }
}
using SistemaGym.Core.Entities;

namespace SistemaGym.Services.Interfaces
{
    public interface IMembresiaService
    {
        Task<IEnumerable<Membresia>> GetAllMembresiasAsync();
        Task<Membresia> GetMembresiaByIdAsync(int id);
        Task InsertMembresia(Membresia membresia);
        Task UpdateMembresia(Membresia membresia);
        Task DeleteMembresia(int id);
    }
}
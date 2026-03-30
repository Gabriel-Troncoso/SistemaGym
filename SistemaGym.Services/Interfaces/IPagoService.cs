using SistemaGym.Core.Entities;

namespace SistemaGym.Services.Interfaces
{
    public interface IPagoService
    {
        Task<IEnumerable<Pago>> GetAllPagosAsync();
        Task<Pago> GetPagoByIdAsync(int id);
        Task InsertPago(Pago pago);
        Task UpdatePago(Pago pago);
        Task DeletePago(int id);
    }
}
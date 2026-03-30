using SistemaGym.Core.Entities;

namespace SistemaGym.Core.Interfaces
{
    public interface IPagoRepository
    {
        Task<IEnumerable<Pago>> GetAllPagosAsync();
        Task<Pago> GetPagoByIdAsync(int id);
        Task InsertPago(Pago pago);
        Task UpdatePago(Pago pago);
        Task DeletePago(Pago pago);
    }
}
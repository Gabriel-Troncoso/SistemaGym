using SistemaGym.Core.CustomEntities;
using SistemaGym.Core.Entities;
using SistemaGym.Core.QueryFilters;

namespace SistemaGym.Services.Interfaces
{
    public interface IPagoService
    {
        Task<IEnumerable<Pago>> GetAllPagosAsync(PagoQueryFilter? filters = null);

        Task<ResponseData> GetAllPagosResponseAsync(PagoQueryFilter? filters = null);

        Task<IEnumerable<Pago>> GetAllPagosDapperAsync(int limit = 10);

        Task<Pago> GetPagoByIdAsync(int id);

        Task InsertPago(Pago pago);

        Task UpdatePago(Pago pago);

        Task DeletePago(int id);
    }
}

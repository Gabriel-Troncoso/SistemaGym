using SistemaGym.Core.Entities;

namespace SistemaGym.Core.Interfaces
{
    public interface IPagoRepository : IBaseRepository<Pago>
    {
        Task<IEnumerable<Pago>> GetAllPagosDapperAsync(int limit = 10);
    }
}

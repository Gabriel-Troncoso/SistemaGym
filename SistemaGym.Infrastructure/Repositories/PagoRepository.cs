using SistemaGym.Core.Entities;
using SistemaGym.Core.Enum;
using SistemaGym.Core.Interfaces;
using SistemaGym.Infrastructure.Data;
using SistemaGym.Infrastructure.Queries;

namespace SistemaGym.Infrastructure.Repositories
{
    public class PagoRepository : BaseRepository<Pago>, IPagoRepository
    {
        private readonly IDapperContext _dapper;

        public PagoRepository(
            SistemaGymContext context,
            IDapperContext dapper)
            : base(context)
        {
            _dapper = dapper;
        }

        public async Task<IEnumerable<Pago>> GetAllPagosDapperAsync(int limit = 10)
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DataBaseProvider.SqlServer => Primero.pagosSql,
                    DataBaseProvider.MySql => Primero.pagosMySql,
                    _ => throw new NotSupportedException("Provider no soportado")
                };

                return await _dapper.QueryAsync<Pago>(sql, new { Limit = limit });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

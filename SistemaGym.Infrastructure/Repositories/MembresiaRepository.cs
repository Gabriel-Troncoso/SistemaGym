using SistemaGym.Core.Entities;
using SistemaGym.Core.Enum;
using SistemaGym.Core.Interfaces;
using SistemaGym.Infrastructure.Data;
using SistemaGym.Infrastructure.Queries;

namespace SistemaGym.Infrastructure.Repositories
{
    public class MembresiaRepository : BaseRepository<Membresia>, IMembresiaRepository
    {
        private readonly IDapperContext _dapper;

        public MembresiaRepository(
            SistemaGymContext context,
            IDapperContext dapper)
            : base(context)
        {
            _dapper = dapper;
        }

        public async Task<IEnumerable<Membresia>> GetAllMembresiasDapperAsync(int limit = 10)
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DataBaseProvider.SqlServer => Primero.membresiasSql,
                    DataBaseProvider.MySql => Primero.membresiasMySql,
                    _ => throw new NotSupportedException("Provider no soportado")
                };

                return await _dapper.QueryAsync<Membresia>(sql, new { Limit = limit });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
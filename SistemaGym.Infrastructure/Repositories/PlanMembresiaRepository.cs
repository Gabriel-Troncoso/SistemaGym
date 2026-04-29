using SistemaGym.Core.Entities;
using SistemaGym.Core.Enum;
using SistemaGym.Core.Interfaces;
using SistemaGym.Infrastructure.Data;
using SistemaGym.Infrastructure.Queries;

namespace SistemaGym.Infrastructure.Repositories
{
    public class PlanMembresiaRepository : BaseRepository<PlanMembresia>, IPlanMembresiaRepository
    {
        private readonly IDapperContext _dapper;

        public PlanMembresiaRepository(
            SistemaGymContext context,
            IDapperContext dapper)
            : base(context)
        {
            _dapper = dapper;
        }

        public async Task<IEnumerable<PlanMembresia>> GetAllPlanesDapperAsync(int limit = 10)
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DataBaseProvider.SqlServer => Primero.planesSql,
                    DataBaseProvider.MySql => Primero.planesMySql,
                    _ => throw new NotSupportedException("Provider no soportado")
                };

                return await _dapper.QueryAsync<PlanMembresia>(sql, new { Limit = limit });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
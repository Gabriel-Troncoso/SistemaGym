using SistemaGym.Core.Enum;
using System.Data;

namespace SistemaGym.Core.Interfaces
{
    public interface IDbConnectionFactory
    {
        DataBaseProvider Provider { get; }
        IDbConnection CreateConnection();
    }
}
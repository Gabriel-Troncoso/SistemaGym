using SistemaGym.Core.Entities;
using System.Data;

namespace SistemaGym.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IClienteRepository ClienteRepository { get; }

        IUsuarioRepository UsuarioRepository { get; }

        IPlanMembresiaRepository PlanMembresiaRepository { get; }

        IMembresiaRepository MembresiaRepository { get; }

        IBaseRepository<Pago> PagoRepository { get; }

        void SaveChanges();

        Task SaveChangesAsync();

        Task BeginTransactionAsync();

        Task CommitAsync();

        Task RollbackAsync();

        IDbConnection? GetDbConnection();

        IDbTransaction? GetDbTransaction();
    }
}

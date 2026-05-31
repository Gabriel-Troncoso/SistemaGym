using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SistemaGym.Core.Entities;
using SistemaGym.Core.Interfaces;
using SistemaGym.Infrastructure.Data;
using System.Data;

namespace SistemaGym.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SistemaGymContext _context;
        private readonly IDapperContext _dapper;

        private readonly IClienteRepository? _clienteRepository;
        private readonly IUsuarioRepository? _usuarioRepository;
        private readonly IPlanMembresiaRepository? _planMembresiaRepository;
        private readonly IMembresiaRepository? _membresiaRepository;
        private readonly IPagoRepository? _pagoRepository;

        private IDbContextTransaction? _efTransaction;

        public UnitOfWork(
            SistemaGymContext context,
            IDapperContext dapper)
        {
            _context = context;
            _dapper = dapper;
        }

        public IClienteRepository ClienteRepository =>
            _clienteRepository ?? new ClienteRepository(_context, _dapper);

        public IUsuarioRepository UsuarioRepository =>
            _usuarioRepository ?? new UsuarioRepository(_context);

        public IPlanMembresiaRepository PlanMembresiaRepository =>
            _planMembresiaRepository ?? new PlanMembresiaRepository(_context, _dapper);

        public IMembresiaRepository MembresiaRepository =>
            _membresiaRepository ?? new MembresiaRepository(_context, _dapper);

        public IPagoRepository PagoRepository =>
            _pagoRepository ?? new PagoRepository(_context, _dapper);

        public async Task BeginTransactionAsync()
        {
            if (_efTransaction == null)
            {
                _efTransaction = await _context.Database.BeginTransactionAsync();

                var connection = _context.Database.GetDbConnection();
                var transaction = _efTransaction.GetDbTransaction();

                _dapper.SetAmbientConnection(connection, transaction);
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();

                if (_efTransaction != null)
                {
                    await _efTransaction.CommitAsync();
                    _efTransaction.Dispose();
                    _efTransaction = null;
                }
            }
            finally
            {
                _dapper.ClearAmbientConnection();
            }
        }

        public async Task RollbackAsync()
        {
            if (_efTransaction != null)
            {
                await _efTransaction.RollbackAsync();
                _efTransaction.Dispose();
                _efTransaction = null;
            }

            _dapper.ClearAmbientConnection();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IDbConnection? GetDbConnection()
        {
            return _context.Database.GetDbConnection();
        }

        public IDbTransaction? GetDbTransaction()
        {
            return _efTransaction?.GetDbTransaction();
        }

        public void Dispose()
        {
            _context.Dispose();
            _efTransaction?.Dispose();
        }
    }
}

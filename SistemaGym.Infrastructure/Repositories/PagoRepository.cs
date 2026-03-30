using Microsoft.EntityFrameworkCore;
using SistemaGym.Core.Entities;
using SistemaGym.Core.Interfaces;
using SistemaGym.Infrastructure.Data;

namespace SistemaGym.Infrastructure.Repositories
{
    public class PagoRepository : IPagoRepository
    {
        private readonly SistemaGymContext _context;

        public PagoRepository(SistemaGymContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pago>> GetAllPagosAsync()
        {
            var pagos = await _context.Pagos
                .Include(x => x.Membresia)
                .ToListAsync();

            return pagos;
        }

        public async Task<Pago> GetPagoByIdAsync(int id)
        {
            var pago = await _context.Pagos
                .Include(x => x.Membresia)
                .FirstOrDefaultAsync(x => x.Id == id);

            return pago;
        }

        public async Task InsertPago(Pago pago)
        {
            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePago(Pago pago)
        {
            _context.Pagos.Update(pago);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePago(Pago pago)
        {
            _context.Pagos.Remove(pago);
            await _context.SaveChangesAsync();
        }
    }
}
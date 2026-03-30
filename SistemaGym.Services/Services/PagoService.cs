using SistemaGym.Core.Entities;
using SistemaGym.Core.Interfaces;
using SistemaGym.Services.Interfaces;

namespace SistemaGym.Services.Services
{
    public class PagoService : IPagoService
    {
        public readonly IBaseRepository<Pago> _pagoRepository;
        public readonly IBaseRepository<Membresia> _membresiaRepository;

        public PagoService(
            IBaseRepository<Pago> pagoRepository,
            IBaseRepository<Membresia> membresiaRepository)
        {
            _pagoRepository = pagoRepository;
            _membresiaRepository = membresiaRepository;
        }

        public async Task<IEnumerable<Pago>> GetAllPagosAsync()
        {
            return await _pagoRepository.GetAll();
        }

        public async Task<Pago> GetPagoByIdAsync(int id)
        {
            return await _pagoRepository.GetById(id);
        }

        public async Task InsertPago(Pago pago)
        {
            var membresia = await _membresiaRepository.GetById(pago.MembresiaId);
            if (membresia == null)
                throw new Exception("La membresía no existe");

            if (!pago.Monto.HasValue || pago.Monto <= 0)
                throw new Exception("El monto del pago debe ser mayor a cero");

            if (string.IsNullOrWhiteSpace(pago.MetodoPago))
                throw new Exception("El método de pago es obligatorio");

            await _pagoRepository.Add(pago);
        }

        public async Task UpdatePago(Pago pago)
        {
            await _pagoRepository.Update(pago);
        }

        public async Task DeletePago(int id)
        {
            await _pagoRepository.Delete(id);
        }
    }
}
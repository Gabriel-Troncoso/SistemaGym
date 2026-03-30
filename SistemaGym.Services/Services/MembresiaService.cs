using SistemaGym.Core.Entities;
using SistemaGym.Core.Interfaces;
using SistemaGym.Services.Interfaces;

namespace SistemaGym.Services.Services
{
    public class MembresiaService : IMembresiaService
    {
        public readonly IBaseRepository<Membresia> _membresiaRepository;
        public readonly IBaseRepository<Cliente> _clienteRepository;
        public readonly IBaseRepository<PlanMembresia> _planRepository;

        public MembresiaService(
            IBaseRepository<Membresia> membresiaRepository,
            IBaseRepository<Cliente> clienteRepository,
            IBaseRepository<PlanMembresia> planRepository)
        {
            _membresiaRepository = membresiaRepository;
            _clienteRepository = clienteRepository;
            _planRepository = planRepository;
        }

        public async Task<IEnumerable<Membresia>> GetAllMembresiasAsync()
        {
            return await _membresiaRepository.GetAll();
        }

        public async Task<Membresia> GetMembresiaByIdAsync(int id)
        {
            return await _membresiaRepository.GetById(id);
        }

        public async Task InsertMembresia(Membresia membresia)
        {
            var cliente = await _clienteRepository.GetById(membresia.ClienteId);
            if (cliente == null)
                throw new Exception("El cliente no existe");

            var plan = await _planRepository.GetById(membresia.PlanMembresiaId);
            if (plan == null)
                throw new Exception("El plan de membresía no existe");

            if (membresia.FechaInicio.HasValue && membresia.FechaFin.HasValue &&
                membresia.FechaFin < membresia.FechaInicio)
                throw new Exception("La fecha fin no puede ser menor que la fecha inicio");

            await _membresiaRepository.Add(membresia);
        }

        public async Task UpdateMembresia(Membresia membresia)
        {
            await _membresiaRepository.Update(membresia);
        }

        public async Task DeleteMembresia(int id)
        {
            await _membresiaRepository.Delete(id);
        }
    }
}
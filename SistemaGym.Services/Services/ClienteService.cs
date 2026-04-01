using SistemaGym.Core.Entities;
using SistemaGym.Core.Interfaces;
using SistemaGym.Services.Interfaces;

namespace SistemaGym.Services.Services
{
    public class ClienteService : IClienteService
    {
        public readonly IBaseRepository<Cliente> _clienteRepository;

        public ClienteService(IBaseRepository<Cliente> clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<IEnumerable<Cliente>> GetAllClientesAsync()
        {
            return await _clienteRepository.GetAll();
        }

        public async Task<Cliente> GetClienteByIdAsync(int id)
        {
            return await _clienteRepository.GetById(id);
        }

        public async Task InsertCliente(Cliente cliente)
        {
            var clientes = await _clienteRepository.GetAll();
            if (clientes.Any(c => c.Ci == cliente.Ci))
                throw new Exception("Ya existe un cliente registrado con ese CI.");

            if (cliente.FechaRegistro.HasValue && cliente.FechaRegistro.Value > DateTime.Now)
                throw new Exception("La fecha de registro no puede ser una fecha futura.");
                
            await _clienteRepository.Add(cliente);
        }

        public async Task UpdateCliente(Cliente cliente)
        {
            var clientes = await _clienteRepository.GetAll();
            if (clientes.Any(c => c.Ci == cliente.Ci && c.Id != cliente.Id))
                throw new Exception("Ya existe otro cliente registrado con ese CI.");

            await _clienteRepository.Update(cliente);
        }

        public async Task DeleteCliente(int id)
        {
            await _clienteRepository.Delete(id);
        }
    }
}

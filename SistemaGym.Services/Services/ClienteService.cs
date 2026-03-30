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
            if (string.IsNullOrWhiteSpace(cliente.Nombre))
                throw new Exception("El nombre del cliente es obligatorio");

            if (string.IsNullOrWhiteSpace(cliente.Apellido))
                throw new Exception("El apellido del cliente es obligatorio");

            if (string.IsNullOrWhiteSpace(cliente.Ci))
                throw new Exception("El CI del cliente es obligatorio");

            await _clienteRepository.Add(cliente);
        }

        public async Task UpdateCliente(Cliente cliente)
        {
            await _clienteRepository.Update(cliente);
        }

        public async Task DeleteCliente(int id)
        {
            await _clienteRepository.Delete(id);
        }
    }
}
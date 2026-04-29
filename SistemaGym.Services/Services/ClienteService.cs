using SistemaGym.Core.Entities;
using SistemaGym.Core.Exceptions;
using SistemaGym.Core.Interfaces;
using SistemaGym.Core.QueryFilters;
using SistemaGym.Services.Interfaces;
using System.Net;

namespace SistemaGym.Services.Services
{
    public class ClienteService : IClienteService
    {
        public readonly IUnitOfWork _unitOfWork;

        public ClienteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Cliente>> GetAllClientesAsync(
            ClienteQueryFilter? filters = null)
        {
            var clientes = await _unitOfWork.ClienteRepository.GetAll();

            if (filters != null)
            {
                if (!string.IsNullOrWhiteSpace(filters.Nombre))
                {
                    clientes = clientes.Where(c =>
                        c.Nombre != null &&
                        c.Nombre.ToLower().Contains(filters.Nombre.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(filters.Apellido))
                {
                    clientes = clientes.Where(c =>
                        c.Apellido != null &&
                        c.Apellido.ToLower().Contains(filters.Apellido.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(filters.Ci))
                {
                    clientes = clientes.Where(c =>
                        c.Ci != null &&
                        c.Ci.ToLower().Contains(filters.Ci.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(filters.Correo))
                {
                    clientes = clientes.Where(c =>
                        c.Correo != null &&
                        c.Correo.ToLower().Contains(filters.Correo.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(filters.Telefono))
                {
                    clientes = clientes.Where(c =>
                        c.Telefono != null &&
                        c.Telefono.ToLower().Contains(filters.Telefono.ToLower()));
                }
            }

            return clientes;
        }

        public async Task<IEnumerable<Cliente>> GetAllClientesDapperAsync(
            int limit = 10)
        {
            return await _unitOfWork.ClienteRepository
                .GetAllClientesDapperAsync(limit);
        }

        public async Task<Cliente> GetClienteByIdAsync(int id)
        {
            return await _unitOfWork.ClienteRepository.GetById(id);
        }

        public async Task InsertCliente(Cliente cliente)
        {
            var clientes = await _unitOfWork.ClienteRepository.GetAll();

            if (clientes.Any(c => c.Ci == cliente.Ci))
            {
                throw new BussinesException(
                    "Ya existe un cliente registrado con ese CI.",
                    HttpStatusCode.BadRequest);
            }

            if (cliente.FechaRegistro.HasValue &&
                cliente.FechaRegistro.Value > DateTime.Now)
            {
                throw new BussinesException(
                    "La fecha de registro no puede ser una fecha futura.",
                    HttpStatusCode.BadRequest);
            }

            await _unitOfWork.ClienteRepository.Add(cliente);
            await _unitOfWork.SaveChangesAsync();
        }

        public void UpdateCliente(Cliente cliente)
        {
            var clientes = _unitOfWork.ClienteRepository.GetAll().Result;

            if (clientes.Any(c => c.Ci == cliente.Ci && c.Id != cliente.Id))
            {
                throw new BussinesException(
                    "Ya existe otro cliente registrado con ese CI.",
                    HttpStatusCode.BadRequest);
            }

            _unitOfWork.ClienteRepository.Update(cliente);
            _unitOfWork.SaveChanges();
        }

        public async Task DeleteCliente(int id)
        {
            await _unitOfWork.ClienteRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
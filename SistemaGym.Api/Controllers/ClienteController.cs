using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaGym.Api.Responses;
using SistemaGym.Core.DTOs;
using SistemaGym.Core.Entities;
using SistemaGym.Core.Exceptions;
using SistemaGym.Core.CustomEntities;
using SistemaGym.Services.Interfaces;
using SistemaGym.Services.Validators;
using SistemaGym.Core.QueryFilters;
using System.Net;

namespace SistemaGym.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _service;
        private readonly IMapper _mapper;
        private readonly CrearClienteDtoValidator _crearValidator;
        private readonly ActualizarClienteDtoValidator _actualizarValidator;

        public ClienteController(
            IClienteService service,
            IMapper mapper,
            CrearClienteDtoValidator crearValidator,
            ActualizarClienteDtoValidator actualizarValidator)
        {
            _service = service;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        #region Sin DTOs

        [HttpGet]
        public async Task<IActionResult> GetClientes([FromQuery] ClienteQueryFilter? filters)
        {
            var clientes = await _service.GetAllClientesAsync(filters);
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClienteById(int id)
        {
            var cliente = await _service.GetClienteByIdAsync(id);
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> InsertCliente(Cliente cliente)
        {
            await _service.InsertCliente(cliente);
            return Created($"api/cliente/{cliente.Id}", cliente);
        }

        [HttpPut]
        public IActionResult UpdateCliente(Cliente cliente)
        {
            _service.UpdateCliente(cliente);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCliente(Cliente cliente)
        {
            await _service.DeleteCliente(cliente.Id);
            return NoContent();
        }

        #endregion

        #region Con DTOs

        [HttpGet("dto")]
        public async Task<IActionResult> GetDtoClientes()
        {
            var clientes = await _service.GetAllClientesAsync();

            var clientesDto = clientes.Select(c => new ClienteDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                Ci = c.Ci,
                Correo = c.Correo,
                Telefono = c.Telefono,
                FechaRegistro = c.FechaRegistro
            });

            return Ok(clientesDto);
        }

        [HttpGet("dto/{id}")]
        public async Task<IActionResult> GetDtoClienteById(int id)
        {
            var cliente = await _service.GetClienteByIdAsync(id);

            if (cliente == null)
                return NotFound("Cliente no encontrado");

            var clienteDto = new ClienteDto
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Ci = cliente.Ci,
                Correo = cliente.Correo,
                Telefono = cliente.Telefono,
                FechaRegistro = cliente.FechaRegistro
            };

            return Ok(clienteDto);
        }

        [HttpPost("dto")]
        public async Task<IActionResult> InsertDtoCliente(ClienteDto clienteDto)
        {
            var cliente = new Cliente
            {
                Id = clienteDto.Id,
                Nombre = clienteDto.Nombre,
                Apellido = clienteDto.Apellido,
                Ci = clienteDto.Ci,
                Correo = clienteDto.Correo,
                Telefono = clienteDto.Telefono,
                FechaRegistro = Convert.ToDateTime(clienteDto.FechaRegistro)
            };

            await _service.InsertCliente(cliente);
            return Created($"api/cliente/{cliente.Id}", cliente);
        }

        [HttpPut("dto/{id}")]
        public async Task<IActionResult> UpdateDtoCliente(int id, [FromBody] ClienteDto clienteDto)
        {
            if (id != clienteDto.Id)
                return BadRequest("El ID del cliente no coincide");

            var cliente = await _service.GetClienteByIdAsync(id);

            if (cliente == null)
                return NotFound("Cliente no encontrado");

            cliente.Nombre = clienteDto.Nombre;
            cliente.Apellido = clienteDto.Apellido;
            cliente.Ci = clienteDto.Ci;
            cliente.Correo = clienteDto.Correo;
            cliente.Telefono = clienteDto.Telefono;
            cliente.FechaRegistro = Convert.ToDateTime(clienteDto.FechaRegistro);

            _service.UpdateCliente(cliente);

            return NoContent();
        }

        [HttpDelete("dto/{id}")]
        public async Task<IActionResult> DeleteDtoCliente(int id)
        {
            var cliente = await _service.GetClienteByIdAsync(id);

            if (cliente == null)
                return NotFound("Cliente no encontrado");

            await _service.DeleteCliente(cliente.Id);

            return NoContent();
        }

        #endregion

        #region Con DTO Mapper

        [HttpGet("dto/mapper")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<ClienteDto>>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetClientesDtoMapper([FromQuery] ClienteQueryFilter? filters)
        {
            var clientes = await _service.GetAllClientesResponseAsync(filters);
            var clientesDto = _mapper.Map<IEnumerable<ClienteDto>>(clientes.Pagination);

            var pagination = new Pagination
            {
                TotalCount = clientes.Pagination.TotalCount,
                PageSize = clientes.Pagination.PageSize,
                CurrentePage = clientes.Pagination.CurrentPage,
                TotalPages = clientes.Pagination.TotalPages,
                HasNextPage = clientes.Pagination.HasNextPage,
                HasPreviousPage = clientes.Pagination.HasPreviousPage
            };

            var response = new ApiResponse<IEnumerable<ClienteDto>>(clientesDto)
            {
                Pagination = pagination,
                Messages = clientes.Messages
            };

            return StatusCode((int)clientes.StatusCode, response);
        }

        [HttpGet("dto/mapper/dapper")]
        public async Task<IActionResult> GetClientesDtoMapperDapper([FromQuery] int limit = 10)
        {
            var clientes = await _service.GetAllClientesDapperAsync(limit);
            var clientesDto = _mapper.Map<IEnumerable<ClienteDto>>(clientes);

            var response = new ApiResponse<IEnumerable<ClienteDto>>(clientesDto);

            return Ok(response);
        }

        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetClienteByIdDtoMapper(int id)
        {
            var cliente = await _service.GetClienteByIdAsync(id);

            if (cliente == null)
                return NotFound("Cliente no encontrado.");

            var clienteDto = _mapper.Map<ClienteDto>(cliente);

            var response = new ApiResponse<ClienteDto>(clienteDto);

            return Ok(response);
        }

        [HttpPost("dto/mapper/")]
        public async Task<IActionResult> InsertClienteDtoMapper(ClienteDto clienteDto)
        {
            var validationResult = await _crearValidator.ValidateAsync(clienteDto);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                var cliente = _mapper.Map<Cliente>(clienteDto);

                await _service.InsertCliente(cliente);

                var response = new ApiResponse<ClienteDto>(clienteDto);

                return Ok(response);
            }
            catch (BussinesException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error inesperado, intente más tarde.", ex);
            }
        }

        [HttpPut("dto/mapper/{id}")]
        public async Task<IActionResult> UpdateClienteDtoMapper(int id, [FromBody] ClienteDto clienteDto)
        {
            if (id != clienteDto.Id)
                return BadRequest("El ID del cliente no coincide.");

            var validationResult = await _actualizarValidator.ValidateAsync(clienteDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    message = "Error de validación",
                    errors = validationResult.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        error = e.ErrorMessage
                    })
                });
            }

            var cliente = await _service.GetClienteByIdAsync(id);

            if (cliente == null)
                return NotFound("Cliente no encontrado.");

            try
            {
                _mapper.Map(clienteDto, cliente);

                _service.UpdateCliente(cliente);

                var response = new ApiResponse<ClienteDto>(clienteDto);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al actualizar el cliente",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeleteClienteDtoMapper(int id)
        {
            var cliente = await _service.GetClienteByIdAsync(id);

            if (cliente == null)
                return NotFound("Cliente no encontrado.");

            await _service.DeleteCliente(id);

            return NoContent();
        }

        #endregion
    }
}

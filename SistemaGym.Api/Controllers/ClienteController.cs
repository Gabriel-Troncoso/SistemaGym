using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SistemaGym.Api.Responses;
using SistemaGym.Core.DTOs;
using SistemaGym.Core.Entities;
using SistemaGym.Services.Interfaces;
using SistemaGym.Services.Validators;

namespace SistemaGym.Api.Controllers
{
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
        public async Task<IActionResult> GetClientes()
        {
            var data = await _service.GetAllClientesAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClienteById(int id)
        {
            var data = await _service.GetClienteByIdAsync(id);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> InsertCliente(Cliente cliente)
        {
            await _service.InsertCliente(cliente);
            return Created($"api/cliente/{cliente.Id}", cliente);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCliente(Cliente cliente)
        {
            await _service.UpdateCliente(cliente);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCliente(Cliente cliente)
        {
            await _service.DeleteCliente(cliente.Id);
            return NoContent();
        }
        #endregion

        #region Con DTOs (manual)
        [HttpGet("dto")]
        public async Task<IActionResult> GetDtoClientes()
        {
            var data = await _service.GetAllClientesAsync();
            var dto = data.Select(c => new ClienteDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                Correo = c.Correo,
                Telefono = c.Telefono,
                FechaRegistro = c.FechaRegistro
                // ajusta los campos según tu ClienteDto
            });
            return Ok(dto);
        }

        [HttpGet("dto/{id}")]
        public async Task<IActionResult> GetDtoClienteById(int id)
        {
            var data = await _service.GetClienteByIdAsync(id);
            var dto = new ClienteDto
            {
                Id = data.Id,
                Nombre = data.Nombre,
                Apellido = data.Apellido,
                Correo = data.Correo,
                Telefono = data.Telefono,
                FechaRegistro = data.FechaRegistro
                // ajusta los campos según tu ClienteDto
            };
            return Ok(dto);
        }

        [HttpPost("dto")]
        public async Task<IActionResult> InsertDtoCliente(ClienteDto clienteDto)
        {
            var entity = new Cliente
            {
                Id = clienteDto.Id,
                Nombre = clienteDto.Nombre,
                Apellido = clienteDto.Apellido,
                Ci = clienteDto.Ci,
                Correo = clienteDto.Correo,
                Telefono = clienteDto.Telefono,
                FechaRegistro = Convert.ToDateTime(clienteDto.FechaRegistro)
                // ajusta los campos según tu entidad
            };
            await _service.InsertCliente(entity);
            return Created($"api/cliente/{entity.Id}", entity);
        }

        [HttpPut("dto/{id}")]
        public async Task<IActionResult> UpdateDtoCliente(int id, [FromBody] ClienteDto clienteDto)
        {
            if (id != clienteDto.Id)
                return BadRequest("El ID del cliente no coincide");

            var entity = await _service.GetClienteByIdAsync(id);
            if (entity == null)
                return NotFound("Cliente no encontrado");

            entity.Nombre = clienteDto.Nombre;
            entity.Apellido = clienteDto.Apellido;
            entity.Ci = clienteDto.Ci;
            entity.Correo = clienteDto.Correo;
            entity.Telefono = clienteDto.Telefono;
            entity.FechaRegistro = Convert.ToDateTime(clienteDto.FechaRegistro);
            // ajusta los campos según tu entidad

            await _service.UpdateCliente(entity);
            return NoContent();
        }

        [HttpDelete("dto/{id}")]
        public async Task<IActionResult> DeleteDtoCliente(int id)
        {
            var entity = await _service.GetClienteByIdAsync(id);
            if (entity == null)
                return NotFound("Cliente no encontrado");

            await _service.DeleteCliente(entity.Id);
            return NoContent();
        }
        #endregion

        #region Con DTO Mapper
        [HttpGet("dto/mapper")]
        public async Task<IActionResult> GetClientesDtoMapper()
        {
            var data = await _service.GetAllClientesAsync();
            var dto = _mapper.Map<IEnumerable<ClienteDto>>(data);
            var response = new ApiResponse<IEnumerable<ClienteDto>>(dto);
            return Ok(response);
        }

        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetClienteByIdDtoMapper(int id)
        {
            var data = await _service.GetClienteByIdAsync(id);
            if (data == null)
                return NotFound("Cliente no encontrado.");

            var dto = _mapper.Map<ClienteDto>(data);
            var response = new ApiResponse<ClienteDto>(dto);
            return Ok(response);
        }

        [HttpPost("dto/mapper")]
        public async Task<IActionResult> InsertClienteDtoMapper(ClienteDto clienteDto)
        {
            var validationResult = await _crearValidator.ValidateAsync(clienteDto);
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

            try
            {
                var entity = _mapper.Map<Cliente>(clienteDto);
                await _service.InsertCliente(entity);

                var response = new ApiResponse<ClienteDto>(clienteDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al crear el cliente",
                    error = ex.Message
                });
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

            var entity = await _service.GetClienteByIdAsync(id);
            if (entity == null)
                return NotFound("Cliente no encontrado.");

            try
            {
                _mapper.Map(clienteDto, entity);

                await _service.UpdateCliente(entity);
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
            var entity = await _service.GetClienteByIdAsync(id);
            if (entity == null)
                return NotFound("Cliente no encontrado.");

            await _service.DeleteCliente(id);
            return NoContent();
        }
        #endregion
    }
}
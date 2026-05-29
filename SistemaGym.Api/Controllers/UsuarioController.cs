using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaGym.Api.Responses;
using SistemaGym.Core.CustomEntities;
using SistemaGym.Core.DTOs;
using SistemaGym.Core.Entities;
using SistemaGym.Core.Exceptions;
using SistemaGym.Core.QueryFilters;
using SistemaGym.Services.Interfaces;
using SistemaGym.Services.Validators;
using System.Net;

namespace SistemaGym.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;
        private readonly IMapper _mapper;
        private readonly CrearUsuarioDtoValidator _crearValidator;
        private readonly ActualizarUsuarioDtoValidator _actualizarValidator;

        public UsuarioController(
            IUsuarioService service,
            IMapper mapper,
            CrearUsuarioDtoValidator crearValidator,
            ActualizarUsuarioDtoValidator actualizarValidator)
        {
            _service = service;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        #region Sin DTOs
        [HttpGet]
        public async Task<IActionResult> GetUsuarios([FromQuery] UsuarioQueryFilter? filters)
        {
            var data = await _service.GetAllUsuariosAsync(filters);
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuarioById(int id)
        {
            var data = await _service.GetUsuarioByIdAsync(id);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> InsertUsuario(Usuario usuario)
        {
            await _service.InsertUsuario(usuario);
            return Created($"api/usuario/{usuario.Id}", usuario);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUsuario(Usuario usuario)
        {
            await _service.UpdateUsuario(usuario);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUsuario(Usuario usuario)
        {
            await _service.DeleteUsuario(usuario.Id);
            return NoContent();
        }
        #endregion

        #region Con DTOs (manual)
        [HttpGet("dto")]
        public async Task<IActionResult> GetDtoUsuarios()
        {
            var data = await _service.GetAllUsuariosAsync();
            var dto = data.Select(u => new UsuarioDto
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Telefono = u.Telefono,
                Email = u.Email,
                Password = u.Password,
                Rol = u.Rol,
                Estado = u.Estado,
                FechaRegistro = u.FechaRegistro
            });
            return Ok(dto);
        }

        [HttpGet("dto/{id}")]
        public async Task<IActionResult> GetDtoUsuarioById(int id)
        {
            var data = await _service.GetUsuarioByIdAsync(id);
            var dto = new UsuarioDto
            {
                Id = data.Id,
                Nombre = data.Nombre,
                Telefono = data.Telefono,
                Email = data.Email,
                Password = data.Password,
                Rol = data.Rol,
                Estado = data.Estado,
                FechaRegistro = data.FechaRegistro
            };
            return Ok(dto);
        }

        [HttpPost("dto")]
        public async Task<IActionResult> InsertDtoUsuario(UsuarioDto usuarioDto)
        {
            var entity = new Usuario
            {
                Id = usuarioDto.Id,
                Nombre = usuarioDto.Nombre,
                Telefono = usuarioDto.Telefono,
                Email = usuarioDto.Email,
                Password = usuarioDto.Password,
                Rol = usuarioDto.Rol,
                Estado = usuarioDto.Estado,
                FechaRegistro = Convert.ToDateTime(usuarioDto.FechaRegistro)
            };
            await _service.InsertUsuario(entity);
            return Created($"api/usuario/{entity.Id}", entity);
        }

        [HttpPut("dto/{id}")]
        public async Task<IActionResult> UpdateDtoUsuario(int id, [FromBody] UsuarioDto usuarioDto)
        {
            if (id != usuarioDto.Id)
                return BadRequest("El ID del usuario no coincide");

            var entity = await _service.GetUsuarioByIdAsync(id);
            if (entity == null)
                return NotFound("Usuario no encontrado");

            entity.Nombre = usuarioDto.Nombre;
            entity.Telefono = usuarioDto.Telefono;
            entity.Email = usuarioDto.Email;
            entity.Password = usuarioDto.Password;
            entity.Rol = usuarioDto.Rol;
            entity.Estado = usuarioDto.Estado;
            entity.FechaRegistro = Convert.ToDateTime(usuarioDto.FechaRegistro);

            await _service.UpdateUsuario(entity);
            return NoContent();
        }

        [HttpDelete("dto/{id}")]
        public async Task<IActionResult> DeleteDtoUsuario(int id)
        {
            var entity = await _service.GetUsuarioByIdAsync(id);
            if (entity == null)
                return NotFound("Usuario no encontrado");

            await _service.DeleteUsuario(entity.Id);
            return NoContent();
        }
        #endregion

        #region Con DTO Mapper
        [HttpGet("dto/mapper")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<UsuarioDto>>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetUsuariosDtoMapper(
            [FromQuery] UsuarioQueryFilter? filters)
        {
            var data = await _service.GetAllUsuariosResponseAsync(filters);
            var dto = _mapper.Map<IEnumerable<UsuarioDto>>(data.Pagination);

            var pagination = new Pagination
            {
                TotalCount = data.Pagination.TotalCount,
                PageSize = data.Pagination.PageSize,
                CurrentePage = data.Pagination.CurrentPage,
                TotalPages = data.Pagination.TotalPages,
                HasNextPage = data.Pagination.HasNextPage,
                HasPreviousPage = data.Pagination.HasPreviousPage
            };

            var response = new ApiResponse<IEnumerable<UsuarioDto>>(dto)
            {
                Pagination = pagination,
                Messages = data.Messages
            };

            return StatusCode((int)data.StatusCode, response);
        }

        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetUsuarioByIdDtoMapper(int id)
        {
            var data = await _service.GetUsuarioByIdAsync(id);
            if (data == null)
                return NotFound("Usuario no encontrado.");

            var dto = _mapper.Map<UsuarioDto>(data);
            var response = new ApiResponse<UsuarioDto>(dto);
            return Ok(response);
        }

        [HttpPost("dto/mapper")]
        public async Task<IActionResult> InsertUsuarioDtoMapper(UsuarioDto usuarioDto)
        {
            var validationResult = await _crearValidator.ValidateAsync(usuarioDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                var entity = _mapper.Map<Usuario>(usuarioDto);
                await _service.InsertUsuario(entity);

                var response = new ApiResponse<UsuarioDto>(usuarioDto);
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
        public async Task<IActionResult> UpdateUsuarioDtoMapper(int id, [FromBody] UsuarioDto usuarioDto)
        {
            if (id != usuarioDto.Id)
                return BadRequest("El ID del usuario no coincide.");

            var validationResult = await _actualizarValidator.ValidateAsync(usuarioDto);
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

            var entity = await _service.GetUsuarioByIdAsync(id);
            if (entity == null)
                return NotFound("Usuario no encontrado.");

            try
            {
                _mapper.Map(usuarioDto, entity);

                await _service.UpdateUsuario(entity);
                var response = new ApiResponse<UsuarioDto>(usuarioDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al actualizar el usuario",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeleteUsuarioDtoMapper(int id)
        {
            var entity = await _service.GetUsuarioByIdAsync(id);
            if (entity == null)
                return NotFound("Usuario no encontrado.");

            await _service.DeleteUsuario(id);
            return NoContent();
        }
        #endregion
    }
}

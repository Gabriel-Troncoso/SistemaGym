using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaGym.Api.Responses;
using SistemaGym.Core.DTOs;
using SistemaGym.Core.Entities;
using SistemaGym.Core.Exceptions;
using SistemaGym.Core.CustomEntities;
using SistemaGym.Core.QueryFilters;
using SistemaGym.Services.Interfaces;
using SistemaGym.Services.Validators;
using System.Net;

namespace SistemaGym.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MembresiaController : ControllerBase
    {
        private readonly IMembresiaService _service;
        private readonly IMapper _mapper;
        private readonly CrearMembresiaDtoValidator _crearValidator;
        private readonly ActualizarMembresiaDtoValidator _actualizarValidator;

        public MembresiaController(
            IMembresiaService service,
            IMapper mapper,
            CrearMembresiaDtoValidator crearValidator,
            ActualizarMembresiaDtoValidator actualizarValidator)
        {
            _service = service;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        #region Sin DTOs

        [HttpGet]
        public async Task<IActionResult> GetMembresias(
            [FromQuery] MembresiaQueryFilter? filters)
        {
            var membresias = await _service.GetAllMembresiasAsync(filters);
            return Ok(membresias);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMembresiaById(int id)
        {
            var membresia = await _service.GetMembresiaByIdAsync(id);
            return Ok(membresia);
        }

        [HttpPost]
        public async Task<IActionResult> InsertMembresia(Membresia membresia)
        {
            await _service.InsertMembresia(membresia);
            return Created($"api/membresia/{membresia.Id}", membresia);
        }

        [HttpPut]
        public IActionResult UpdateMembresia(Membresia membresia)
        {
            _service.UpdateMembresia(membresia);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMembresia(Membresia membresia)
        {
            await _service.DeleteMembresia(membresia.Id);
            return NoContent();
        }

        #endregion

        #region Con DTOs

        [HttpGet("dto")]
        public async Task<IActionResult> GetDtoMembresias()
        {
            var membresias = await _service.GetAllMembresiasAsync();

            var membresiasDto = membresias.Select(m => new MembresiaDto
            {
                Id = m.Id,
                ClienteId = m.ClienteId,
                PlanMembresiaId = m.PlanMembresiaId,
                FechaInicio = m.FechaInicio,
                FechaFin = m.FechaFin,
                Estado = m.Estado
            });

            return Ok(membresiasDto);
        }

        [HttpGet("dto/{id}")]
        public async Task<IActionResult> GetDtoMembresiaById(int id)
        {
            var membresia = await _service.GetMembresiaByIdAsync(id);

            if (membresia == null)
                return NotFound("Membresía no encontrada");

            var membresiaDto = new MembresiaDto
            {
                Id = membresia.Id,
                ClienteId = membresia.ClienteId,
                PlanMembresiaId = membresia.PlanMembresiaId,
                FechaInicio = membresia.FechaInicio,
                FechaFin = membresia.FechaFin,
                Estado = membresia.Estado
            };

            return Ok(membresiaDto);
        }

        [HttpPost("dto")]
        public async Task<IActionResult> InsertDtoMembresia(MembresiaDto membresiaDto)
        {
            var membresia = new Membresia
            {
                Id = membresiaDto.Id,
                ClienteId = membresiaDto.ClienteId,
                PlanMembresiaId = membresiaDto.PlanMembresiaId,
                FechaInicio = membresiaDto.FechaInicio,
                FechaFin = membresiaDto.FechaFin,
                Estado = membresiaDto.Estado
            };

            await _service.InsertMembresia(membresia);
            return Created($"api/membresia/{membresia.Id}", membresia);
        }

        [HttpPut("dto/{id}")]
        public async Task<IActionResult> UpdateDtoMembresia(
            int id,
            [FromBody] MembresiaDto membresiaDto)
        {
            if (id != membresiaDto.Id)
                return BadRequest("El ID de la membresía no coincide");

            var membresia = await _service.GetMembresiaByIdAsync(id);

            if (membresia == null)
                return NotFound("Membresía no encontrada");

            membresia.ClienteId = membresiaDto.ClienteId;
            membresia.PlanMembresiaId = membresiaDto.PlanMembresiaId;
            membresia.FechaInicio = membresiaDto.FechaInicio;
            membresia.FechaFin = membresiaDto.FechaFin;
            membresia.Estado = membresiaDto.Estado;

            _service.UpdateMembresia(membresia);

            return NoContent();
        }

        [HttpDelete("dto/{id}")]
        public async Task<IActionResult> DeleteDtoMembresia(int id)
        {
            var membresia = await _service.GetMembresiaByIdAsync(id);

            if (membresia == null)
                return NotFound("Membresía no encontrada");

            await _service.DeleteMembresia(membresia.Id);

            return NoContent();
        }

        #endregion

        #region Con DTO Mapper

        [HttpGet("dto/mapper")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<MembresiaDto>>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetMembresiasDtoMapper(
            [FromQuery] MembresiaQueryFilter? filters)
        {
            var membresias = await _service.GetAllMembresiasResponseAsync(filters);
            var membresiasDto = _mapper.Map<IEnumerable<MembresiaDto>>(membresias.Pagination);

            var pagination = new Pagination
            {
                TotalCount = membresias.Pagination.TotalCount,
                PageSize = membresias.Pagination.PageSize,
                CurrentePage = membresias.Pagination.CurrentPage,
                TotalPages = membresias.Pagination.TotalPages,
                HasNextPage = membresias.Pagination.HasNextPage,
                HasPreviousPage = membresias.Pagination.HasPreviousPage
            };

            var response = new ApiResponse<IEnumerable<MembresiaDto>>(membresiasDto)
            {
                Pagination = pagination,
                Messages = membresias.Messages
            };

            return StatusCode((int)membresias.StatusCode, response);
        }

        [HttpGet("dto/mapper/dapper")]
        public async Task<IActionResult> GetMembresiasDtoMapperDapper(
            [FromQuery] int limit = 10)
        {
            var membresias = await _service.GetAllMembresiasDapperAsync(limit);
            var membresiasDto = _mapper.Map<IEnumerable<MembresiaDto>>(membresias);

            var response = new ApiResponse<IEnumerable<MembresiaDto>>(membresiasDto);

            return Ok(response);
        }

        [HttpGet("dto/mapper/{id:int}")]
        public async Task<IActionResult> GetMembresiaByIdDtoMapper(int id)
        {
            var membresia = await _service.GetMembresiaByIdAsync(id);

            if (membresia == null)
                return NotFound("Membresía no encontrada.");

            var membresiaDto = _mapper.Map<MembresiaDto>(membresia);

            var response = new ApiResponse<MembresiaDto>(membresiaDto);

            return Ok(response);
        }

        [HttpPost("dto/mapper")]
        public async Task<IActionResult> InsertMembresiaDtoMapper(MembresiaDto membresiaDto)
        {
            var validationResult = await _crearValidator.ValidateAsync(membresiaDto);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                var membresia = _mapper.Map<Membresia>(membresiaDto);

                await _service.InsertMembresia(membresia);

                var response = new ApiResponse<MembresiaDto>(membresiaDto);

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

        [HttpPut("dto/mapper/{id:int}")]
        public async Task<IActionResult> UpdateMembresiaDtoMapper(
            int id,
            [FromBody] MembresiaDto membresiaDto)
        {
            if (id != membresiaDto.Id)
                return BadRequest("El ID de la membresía no coincide.");

            var validationResult = await _actualizarValidator.ValidateAsync(membresiaDto);

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

            var membresia = await _service.GetMembresiaByIdAsync(id);

            if (membresia == null)
                return NotFound("Membresía no encontrada.");

            try
            {
                _mapper.Map(membresiaDto, membresia);

                _service.UpdateMembresia(membresia);

                var response = new ApiResponse<MembresiaDto>(membresiaDto);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al actualizar la membresía",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("dto/mapper/{id:int}")]
        public async Task<IActionResult> DeleteMembresiaDtoMapper(int id)
        {
            var membresia = await _service.GetMembresiaByIdAsync(id);

            if (membresia == null)
                return NotFound("Membresía no encontrada.");

            await _service.DeleteMembresia(id);

            return NoContent();
        }

        #endregion
    }
}

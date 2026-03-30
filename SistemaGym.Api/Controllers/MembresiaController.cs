using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SistemaGym.Api.Responses;
using SistemaGym.Core.DTOs;
using SistemaGym.Core.Entities;
using SistemaGym.Services.Interfaces;
using SistemaGym.Services.Validators;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace SistemaGym.Api.Controllers
{
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
        public async Task<IActionResult> GetMembresias()
        {
            var data = await _service.GetAllMembresiasAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMembresiaById(int id)
        {
            var data = await _service.GetMembresiaByIdAsync(id);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> InsertMembresia(Membresia membresia)
        {
            await _service.InsertMembresia(membresia);
            return Created($"api/membresia/{membresia.Id}", membresia);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMembresia(Membresia membresia)
        {
            await _service.UpdateMembresia(membresia);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMembresia(Membresia membresia)
        {
            await _service.DeleteMembresia(membresia.Id);
            return NoContent();
        }
        #endregion

        #region Con DTOs (manual)
        [HttpGet("dto")]
        public async Task<IActionResult> GetDtoMembresias()
        {
            var data = await _service.GetAllMembresiasAsync();
            var dto = data.Select(m => new MembresiaDto
            {
                Id = m.Id,
                ClienteId = m.ClienteId,
                PlanMembresiaId = m.PlanMembresiaId,
                FechaInicio = m.FechaInicio,
                FechaFin = m.FechaFin,
                Estado = m.Estado
            });
            return Ok(dto);
        }

        [HttpGet("dto/{id}")]
        public async Task<IActionResult> GetDtoMembresiaById(int id)
        {
            var data = await _service.GetMembresiaByIdAsync(id);
            var dto = new MembresiaDto
            {
                Id = data.Id,
                ClienteId = data.ClienteId,
                PlanMembresiaId = data.PlanMembresiaId,
                FechaInicio = data.FechaInicio,
                FechaFin = data.FechaFin,
                Estado = data.Estado
            };
            return Ok(dto);
        }

        [HttpPost("dto")]
        public async Task<IActionResult> InsertDtoMembresia(MembresiaDto membresiaDto)
        {
            var entity = new Membresia
            {
                Id = membresiaDto.Id,
                ClienteId = membresiaDto.ClienteId,
                PlanMembresiaId = membresiaDto.PlanMembresiaId,
                FechaInicio = Convert.ToDateTime(membresiaDto.FechaInicio),
                FechaFin = Convert.ToDateTime(membresiaDto.FechaFin),
                Estado = membresiaDto.Estado
            };
            await _service.InsertMembresia(entity);
            return Created($"api/membresia/{entity.Id}", entity);
        }

        [HttpPut("dto/{id}")]
        public async Task<IActionResult> UpdateDtoMembresia(int id, [FromBody] MembresiaDto membresiaDto)
        {
            if (id != membresiaDto.Id)
                return BadRequest("El ID de la membresía no coincide");

            var entity = await _service.GetMembresiaByIdAsync(id);
            if (entity == null)
                return NotFound("Membresía no encontrada");

            entity.ClienteId = membresiaDto.ClienteId;
            entity.PlanMembresiaId = membresiaDto.PlanMembresiaId;
            entity.FechaInicio = Convert.ToDateTime(membresiaDto.FechaInicio);
            entity.FechaFin = Convert.ToDateTime(membresiaDto.FechaFin);
            entity.Estado = membresiaDto.Estado;

            await _service.UpdateMembresia(entity);
            return NoContent();
        }

        [HttpDelete("dto/{id}")]
        public async Task<IActionResult> DeleteDtoMembresia(int id)
        {
            var entity = await _service.GetMembresiaByIdAsync(id);
            if (entity == null)
                return NotFound("Membresía no encontrada");

            await _service.DeleteMembresia(entity.Id);
            return NoContent();
        }
        #endregion

        #region Con DTO Mapper
        [HttpGet("dto/mapper")]
        public async Task<IActionResult> GetMembresiasDtoMapper()
        {
            var data = await _service.GetAllMembresiasAsync();
            var dto = _mapper.Map<IEnumerable<MembresiaDto>>(data);
            var response = new ApiResponse<IEnumerable<MembresiaDto>>(dto);
            return Ok(response);
        }

        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetMembresiaByIdDtoMapper(int id)
        {
            var data = await _service.GetMembresiaByIdAsync(id);
            if (data == null)
                return NotFound("Membresía no encontrada.");

            var dto = _mapper.Map<MembresiaDto>(data);
            var response = new ApiResponse<MembresiaDto>(dto);
            return Ok(response);
        }

        [HttpPost("dto/mapper")]
        public async Task<IActionResult> InsertMembresiaDtoMapper(MembresiaDto membresiaDto)
        {
            var validationResult = await _crearValidator.ValidateAsync(membresiaDto);
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
                var entity = _mapper.Map<Membresia>(membresiaDto);
                await _service.InsertMembresia(entity);

                var response = new ApiResponse<MembresiaDto>(membresiaDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al crear la membresía",
                    error = ex.Message
                });
            }
        }

        [HttpPut("dto/mapper/{id}")]
        public async Task<IActionResult> UpdateMembresiaDtoMapper(int id, [FromBody] MembresiaDto membresiaDto)
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

            var entity = await _service.GetMembresiaByIdAsync(id);
            if (entity == null)
                return NotFound("Membresía no encontrada.");

            try
            {
                _mapper.Map(membresiaDto, entity);

                await _service.UpdateMembresia(entity);
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

        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeleteMembresiaDtoMapper(int id)
        {
            var entity = await _service.GetMembresiaByIdAsync(id);
            if (entity == null)
                return NotFound("Membresía no encontrada.");

            await _service.DeleteMembresia(id);
            return NoContent();
        }
        #endregion
    }
}
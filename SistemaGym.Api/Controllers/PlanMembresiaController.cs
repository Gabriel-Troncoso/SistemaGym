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
    public class PlanMembresiaController : ControllerBase
    {
        private readonly IPlanMembresiaService _service;
        private readonly IMapper _mapper;
        private readonly CrearPlanMembresiaDtoValidator _crearValidator;
        private readonly ActualizarPlanMembresiaDtoValidator _actualizarValidator;

        public PlanMembresiaController(
            IPlanMembresiaService service,
            IMapper mapper,
            CrearPlanMembresiaDtoValidator crearValidator,
            ActualizarPlanMembresiaDtoValidator actualizarValidator)
        {
            _service = service;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        #region Sin DTOs
        [HttpGet]
        public async Task<IActionResult> GetPlanes()
        {
            var data = await _service.GetAllPlanesAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlanById(int id)
        {
            var data = await _service.GetPlanByIdAsync(id);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> InsertPlan(PlanMembresia planMembresia)
        {
            await _service.InsertPlan(planMembresia);
            return Created($"api/planmembresia/{planMembresia.Id}", planMembresia);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePlan(PlanMembresia planMembresia)
        {
            await _service.UpdatePlan(planMembresia);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePlan(PlanMembresia planMembresia)
        {
            await _service.DeletePlan(planMembresia.Id);
            return NoContent();
        }
        #endregion

        #region Con DTOs (manual)
        [HttpGet("dto")]
        public async Task<IActionResult> GetDtoPlanes()
        {
            var data = await _service.GetAllPlanesAsync();
            var dto = data.Select(p => new PlanMembresiaDto
            {
                Id = p.Id,
                NombrePlan = p.NombrePlan,
                Descripcion = p.Descripcion,
                DuracionDias = p.DuracionDias,
                Precio = p.Precio,
                Estado = p.Estado
            });
            return Ok(dto);
        }

        [HttpGet("dto/{id}")]
        public async Task<IActionResult> GetDtoPlanById(int id)
        {
            var data = await _service.GetPlanByIdAsync(id);
            var dto = new PlanMembresiaDto
            {
                Id = data.Id,
                NombrePlan = data.NombrePlan,
                Descripcion = data.Descripcion,
                DuracionDias = data.DuracionDias,
                Precio = data.Precio,
                Estado = data.Estado
            };
            return Ok(dto);
        }

        [HttpPost("dto")]
        public async Task<IActionResult> InsertDtoPlan(PlanMembresiaDto planDto)
        {
            var entity = new PlanMembresia
            {
                Id = planDto.Id,
                NombrePlan = planDto.NombrePlan,
                Descripcion = planDto.Descripcion,
                DuracionDias = planDto.DuracionDias,
                Precio = planDto.Precio,
                Estado = planDto.Estado
            };
            await _service.InsertPlan(entity);
            return Created($"api/planmembresia/{entity.Id}", entity);
        }

        [HttpPut("dto/{id}")]
        public async Task<IActionResult> UpdateDtoPlan(int id, [FromBody] PlanMembresiaDto planDto)
        {
            if (id != planDto.Id)
                return BadRequest("El ID del plan no coincide");

            var entity = await _service.GetPlanByIdAsync(id);
            if (entity == null)
                return NotFound("Plan de membresía no encontrado");

            entity.NombrePlan = planDto.NombrePlan;
            entity.Descripcion = planDto.Descripcion;
            entity.DuracionDias = planDto.DuracionDias;
            entity.Precio = planDto.Precio;
            entity.Estado = planDto.Estado;

            await _service.UpdatePlan(entity);
            return NoContent();
        }

        [HttpDelete("dto/{id}")]
        public async Task<IActionResult> DeleteDtoPlan(int id)
        {
            var entity = await _service.GetPlanByIdAsync(id);
            if (entity == null)
                return NotFound("Plan de membresía no encontrado");

            await _service.DeletePlan(entity.Id);
            return NoContent();
        }
        #endregion

        #region Con DTO Mapper
        [HttpGet("dto/mapper")]
        public async Task<IActionResult> GetPlanesDtoMapper()
        {
            var data = await _service.GetAllPlanesAsync();
            var dto = _mapper.Map<IEnumerable<PlanMembresiaDto>>(data);
            var response = new ApiResponse<IEnumerable<PlanMembresiaDto>>(dto);
            return Ok(response);
        }

        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetPlanByIdDtoMapper(int id)
        {
            var data = await _service.GetPlanByIdAsync(id);
            if (data == null)
                return NotFound("Plan de membresía no encontrado.");

            var dto = _mapper.Map<PlanMembresiaDto>(data);
            var response = new ApiResponse<PlanMembresiaDto>(dto);
            return Ok(response);
        }

        [HttpPost("dto/mapper")]
        public async Task<IActionResult> InsertPlanDtoMapper(PlanMembresiaDto planDto)
        {
            var validationResult = await _crearValidator.ValidateAsync(planDto);
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
                var entity = _mapper.Map<PlanMembresia>(planDto);
                await _service.InsertPlan(entity);

                var response = new ApiResponse<PlanMembresiaDto>(planDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al crear el plan de membresía",
                    error = ex.Message
                });
            }
        }

        [HttpPut("dto/mapper/{id}")]
        public async Task<IActionResult> UpdatePlanDtoMapper(int id, [FromBody] PlanMembresiaDto planDto)
        {
            if (id != planDto.Id)
                return BadRequest("El ID del plan no coincide.");

            var validationResult = await _actualizarValidator.ValidateAsync(planDto);
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

            var entity = await _service.GetPlanByIdAsync(id);
            if (entity == null)
                return NotFound("Plan de membresía no encontrado.");

            try
            {
                _mapper.Map(planDto, entity);

                await _service.UpdatePlan(entity);
                var response = new ApiResponse<PlanMembresiaDto>(planDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al actualizar el plan de membresía",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeletePlanDtoMapper(int id)
        {
            var entity = await _service.GetPlanByIdAsync(id);
            if (entity == null)
                return NotFound("Plan de membresía no encontrado.");

            await _service.DeletePlan(id);
            return NoContent();
        }
        #endregion
    }
}
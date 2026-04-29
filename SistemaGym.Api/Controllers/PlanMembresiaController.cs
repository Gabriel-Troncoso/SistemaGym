using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SistemaGym.Api.Responses;
using SistemaGym.Core.DTOs;
using SistemaGym.Core.Entities;
using SistemaGym.Core.Exceptions;
using SistemaGym.Core.QueryFilters;
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
        public async Task<IActionResult> GetPlanes([FromQuery] PlanMembresiaQueryFilter? filters)
        {
            var planes = await _service.GetAllPlanesAsync(filters);
            return Ok(planes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlanById(int id)
        {
            var plan = await _service.GetPlanByIdAsync(id);
            return Ok(plan);
        }

        [HttpPost]
        public async Task<IActionResult> InsertPlan(PlanMembresia planMembresia)
        {
            await _service.InsertPlan(planMembresia);
            return Created($"api/planmembresia/{planMembresia.Id}", planMembresia);
        }

        [HttpPut]
        public IActionResult UpdatePlan(PlanMembresia planMembresia)
        {
            _service.UpdatePlan(planMembresia);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePlan(PlanMembresia planMembresia)
        {
            await _service.DeletePlan(planMembresia.Id);
            return NoContent();
        }

        #endregion

        #region Con DTOs

        [HttpGet("dto")]
        public async Task<IActionResult> GetDtoPlanes()
        {
            var planes = await _service.GetAllPlanesAsync();

            var planesDto = planes.Select(p => new PlanMembresiaDto
            {
                Id = p.Id,
                NombrePlan = p.NombrePlan,
                Descripcion = p.Descripcion,
                DuracionDias = p.DuracionDias,
                Precio = p.Precio,
                Estado = p.Estado
            });

            return Ok(planesDto);
        }

        [HttpGet("dto/{id}")]
        public async Task<IActionResult> GetDtoPlanById(int id)
        {
            var plan = await _service.GetPlanByIdAsync(id);

            if (plan == null)
                return NotFound("Plan de membresía no encontrado");

            var planDto = new PlanMembresiaDto
            {
                Id = plan.Id,
                NombrePlan = plan.NombrePlan,
                Descripcion = plan.Descripcion,
                DuracionDias = plan.DuracionDias,
                Precio = plan.Precio,
                Estado = plan.Estado
            };

            return Ok(planDto);
        }

        [HttpPost("dto")]
        public async Task<IActionResult> InsertDtoPlan(PlanMembresiaDto planDto)
        {
            var plan = new PlanMembresia
            {
                Id = planDto.Id,
                NombrePlan = planDto.NombrePlan,
                Descripcion = planDto.Descripcion,
                DuracionDias = planDto.DuracionDias,
                Precio = planDto.Precio,
                Estado = planDto.Estado
            };

            await _service.InsertPlan(plan);
            return Created($"api/planmembresia/{plan.Id}", plan);
        }

        [HttpPut("dto/{id}")]
        public async Task<IActionResult> UpdateDtoPlan(int id, [FromBody] PlanMembresiaDto planDto)
        {
            if (id != planDto.Id)
                return BadRequest("El ID del plan no coincide");

            var plan = await _service.GetPlanByIdAsync(id);

            if (plan == null)
                return NotFound("Plan de membresía no encontrado");

            plan.NombrePlan = planDto.NombrePlan;
            plan.Descripcion = planDto.Descripcion;
            plan.DuracionDias = planDto.DuracionDias;
            plan.Precio = planDto.Precio;
            plan.Estado = planDto.Estado;

            _service.UpdatePlan(plan);

            return NoContent();
        }

        [HttpDelete("dto/{id}")]
        public async Task<IActionResult> DeleteDtoPlan(int id)
        {
            var plan = await _service.GetPlanByIdAsync(id);

            if (plan == null)
                return NotFound("Plan de membresía no encontrado");

            await _service.DeletePlan(plan.Id);

            return NoContent();
        }

        #endregion

        #region Con DTO Mapper

        [HttpGet("dto/mapper")]
        public async Task<IActionResult> GetPlanesDtoMapper(
            [FromQuery] PlanMembresiaQueryFilter? filters)
        {
            var planes = await _service.GetAllPlanesAsync(filters);
            var planesDto = _mapper.Map<IEnumerable<PlanMembresiaDto>>(planes);

            var response = new ApiResponse<IEnumerable<PlanMembresiaDto>>(planesDto);

            return Ok(response);
        }

        [HttpGet("dto/mapper/dapper")]
        public async Task<IActionResult> GetPlanesDtoMapperDapper(
            [FromQuery] int limit = 10)
        {
            var planes = await _service.GetAllPlanesDapperAsync(limit);
            var planesDto = _mapper.Map<IEnumerable<PlanMembresiaDto>>(planes);

            var response = new ApiResponse<IEnumerable<PlanMembresiaDto>>(planesDto);

            return Ok(response);
        }

        [HttpGet("dto/mapper/{id:int}")]
        public async Task<IActionResult> GetPlanByIdDtoMapper(int id)
        {
            var plan = await _service.GetPlanByIdAsync(id);

            if (plan == null)
                return NotFound("Plan de membresía no encontrado.");

            var planDto = _mapper.Map<PlanMembresiaDto>(plan);

            var response = new ApiResponse<PlanMembresiaDto>(planDto);

            return Ok(response);
        }

        [HttpPost("dto/mapper/")]
        public async Task<IActionResult> InsertPlanDtoMapper(PlanMembresiaDto planDto)
        {
            var validationResult = await _crearValidator.ValidateAsync(planDto);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                var plan = _mapper.Map<PlanMembresia>(planDto);

                await _service.InsertPlan(plan);

                var response = new ApiResponse<PlanMembresiaDto>(planDto);

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
        public async Task<IActionResult> UpdatePlanDtoMapper(
            int id,
            [FromBody] PlanMembresiaDto planDto)
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

            var plan = await _service.GetPlanByIdAsync(id);

            if (plan == null)
                return NotFound("Plan de membresía no encontrado.");

            try
            {
                _mapper.Map(planDto, plan);

                _service.UpdatePlan(plan);

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

        [HttpDelete("dto/mapper/{id:int}")]
        public async Task<IActionResult> DeletePlanDtoMapper(int id)
        {
            var plan = await _service.GetPlanByIdAsync(id);

            if (plan == null)
                return NotFound("Plan de membresía no encontrado.");

            await _service.DeletePlan(id);

            return NoContent();
        }

        #endregion
    }
}
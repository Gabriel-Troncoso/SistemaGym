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
    public class PagoController : ControllerBase
    {
        private readonly IPagoService _service;
        private readonly IMapper _mapper;
        private readonly CrearPagoDtoValidator _crearValidator;
        private readonly ActualizarPagoDtoValidator _actualizarValidator;

        public PagoController(
            IPagoService service,
            IMapper mapper,
            CrearPagoDtoValidator crearValidator,
            ActualizarPagoDtoValidator actualizarValidator)
        {
            _service = service;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        #region Sin DTOs
        [HttpGet]
        public async Task<IActionResult> GetPagos([FromQuery] PagoQueryFilter? filters)
        {
            var data = await _service.GetAllPagosAsync(filters);
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPagoById(int id)
        {
            var data = await _service.GetPagoByIdAsync(id);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> InsertPago(Pago pago)
        {
            await _service.InsertPago(pago);
            return Created($"api/pago/{pago.Id}", pago);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePago(Pago pago)
        {
            await _service.UpdatePago(pago);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePago(Pago pago)
        {
            await _service.DeletePago(pago.Id);
            return NoContent();
        }
        #endregion

        #region Con DTOs (manual)
        [HttpGet("dto")]
        public async Task<IActionResult> GetDtoPagos()
        {
            var data = await _service.GetAllPagosAsync();
            var dto = data.Select(p => new PagoDto
            {
                Id = p.Id,
                MembresiaId = p.MembresiaId,
                Monto = p.Monto,
                FechaPago = p.FechaPago,
                MetodoPago = p.MetodoPago,
                Estado = p.Estado
            });
            return Ok(dto);
        }

        [HttpGet("dto/{id}")]
        public async Task<IActionResult> GetDtoPagoById(int id)
        {
            var data = await _service.GetPagoByIdAsync(id);
            var dto = new PagoDto
            {
                Id = data.Id,
                MembresiaId = data.MembresiaId,
                Monto = data.Monto,
                FechaPago = data.FechaPago,
                MetodoPago = data.MetodoPago,
                Estado = data.Estado
            };
            return Ok(dto);
        }

        [HttpPost("dto")]
        public async Task<IActionResult> InsertDtoPago(PagoDto pagoDto)
        {
            var entity = new Pago
            {
                Id = pagoDto.Id,
                MembresiaId = pagoDto.MembresiaId,
                Monto = pagoDto.Monto,
                FechaPago = Convert.ToDateTime(pagoDto.FechaPago),
                MetodoPago = pagoDto.MetodoPago,
                Estado = pagoDto.Estado
            };
            await _service.InsertPago(entity);
            return Created($"api/pago/{entity.Id}", entity);
        }

        [HttpPut("dto/{id}")]
        public async Task<IActionResult> UpdateDtoPago(int id, [FromBody] PagoDto pagoDto)
        {
            if (id != pagoDto.Id)
                return BadRequest("El ID del pago no coincide");

            var entity = await _service.GetPagoByIdAsync(id);
            if (entity == null)
                return NotFound("Pago no encontrado");

            entity.MembresiaId = pagoDto.MembresiaId;
            entity.Monto = pagoDto.Monto;
            entity.FechaPago = Convert.ToDateTime(pagoDto.FechaPago);
            entity.MetodoPago = pagoDto.MetodoPago;
            entity.Estado = pagoDto.Estado;

            await _service.UpdatePago(entity);
            return NoContent();
        }

        [HttpDelete("dto/{id}")]
        public async Task<IActionResult> DeleteDtoPago(int id)
        {
            var entity = await _service.GetPagoByIdAsync(id);
            if (entity == null)
                return NotFound("Pago no encontrado");

            await _service.DeletePago(entity.Id);
            return NoContent();
        }
        #endregion

        #region Con DTO Mapper
        [HttpGet("dto/mapper")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<PagoDto>>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetPagosDtoMapper(
            [FromQuery] PagoQueryFilter? filters)
        {
            var data = await _service.GetAllPagosResponseAsync(filters);
            var dto = _mapper.Map<IEnumerable<PagoDto>>(data.Pagination);

            var pagination = new Pagination
            {
                TotalCount = data.Pagination.TotalCount,
                PageSize = data.Pagination.PageSize,
                CurrentePage = data.Pagination.CurrentPage,
                TotalPages = data.Pagination.TotalPages,
                HasNextPage = data.Pagination.HasNextPage,
                HasPreviousPage = data.Pagination.HasPreviousPage
            };

            var response = new ApiResponse<IEnumerable<PagoDto>>(dto)
            {
                Pagination = pagination,
                Messages = data.Messages
            };

            return StatusCode((int)data.StatusCode, response);
        }

        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetPagoByIdDtoMapper(int id)
        {
            var data = await _service.GetPagoByIdAsync(id);
            if (data == null)
                return NotFound("Pago no encontrado.");

            var dto = _mapper.Map<PagoDto>(data);
            var response = new ApiResponse<PagoDto>(dto);
            return Ok(response);
        }

        [HttpPost("dto/mapper")]
        public async Task<IActionResult> InsertPagoDtoMapper(PagoDto pagoDto)
        {
            var validationResult = await _crearValidator.ValidateAsync(pagoDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                var entity = _mapper.Map<Pago>(pagoDto);
                await _service.InsertPago(entity);

                var response = new ApiResponse<PagoDto>(pagoDto);
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
        public async Task<IActionResult> UpdatePagoDtoMapper(int id, [FromBody] PagoDto pagoDto)
        {
            if (id != pagoDto.Id)
                return BadRequest("El ID del pago no coincide.");

            var validationResult = await _actualizarValidator.ValidateAsync(pagoDto);
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

            var entity = await _service.GetPagoByIdAsync(id);
            if (entity == null)
                return NotFound("Pago no encontrado.");

            try
            {
                _mapper.Map(pagoDto, entity);

                await _service.UpdatePago(entity);
                var response = new ApiResponse<PagoDto>(pagoDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al actualizar el pago",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeletePagoDtoMapper(int id)
        {
            var entity = await _service.GetPagoByIdAsync(id);
            if (entity == null)
                return NotFound("Pago no encontrado.");

            await _service.DeletePago(id);
            return NoContent();
        }
        #endregion
    }
}

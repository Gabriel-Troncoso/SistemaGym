using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SistemaGym.Core.DTOs;
using SistemaGym.Core.Entities;
using SistemaGym.Core.Interfaces;

namespace SistemaGym.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

        public ClienteController(IClienteRepository clienteRepository, IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
        }

        #region Sin DTOs
        [HttpGet]
        public async Task<IActionResult> GetClientes()
        {
            var clientes = await _clienteRepository.GetAllClientesAsync();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClienteById(int id)
        {
            var cliente = await _clienteRepository.GetClienteByIdAsync(id);
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> InsertCliente(Cliente cliente)
        {
            await _clienteRepository.InsertCliente(cliente);
            return Created($"api/cliente/{cliente.Id}", cliente);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCliente(Cliente cliente)
        {
            await _clienteRepository.UpdateCliente(cliente);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCliente(Cliente cliente)
        {
            await _clienteRepository.DeleteCliente(cliente);
            return NoContent();
        }
        #endregion

        #region Con DTO Mapper
        [HttpGet("dto")]
        public async Task<IActionResult> GetDtoClientes()
        {
            var clientes = await _clienteRepository.GetAllClientesAsync();
            var clientesDto = _mapper.Map<IEnumerable<ClienteDto>>(clientes);
            return Ok(clientesDto);
        }

        [HttpGet("dto/{id}")]
        public async Task<IActionResult> GetDtoClienteById(int id)
        {
            var cliente = await _clienteRepository.GetClienteByIdAsync(id);
            var clienteDto = _mapper.Map<ClienteDto>(cliente);
            return Ok(clienteDto);
        }

        [HttpPost("dto")]
        public async Task<IActionResult> InsertDtoCliente(ClienteDto clienteDto)
        {
            var cliente = _mapper.Map<Cliente>(clienteDto);
            await _clienteRepository.InsertCliente(cliente);
            return Created($"api/cliente/{cliente.Id}", cliente);
        }

        [HttpPut("dto/{id}")]
        public async Task<IActionResult> UpdateDtoCliente(int id, [FromBody] ClienteDto clienteDto)
        {
            if (id != clienteDto.Id)
                return BadRequest("El ID no coincide");

            var cliente = await _clienteRepository.GetClienteByIdAsync(id);
            if (cliente == null)
                return NotFound();

            _mapper.Map(clienteDto, cliente);

            await _clienteRepository.UpdateCliente(cliente);
            return NoContent();
        }

        [HttpDelete("dto/{id}")]
        public async Task<IActionResult> DeleteDtoCliente(int id)
        {
            var cliente = await _clienteRepository.GetClienteByIdAsync(id);
            if (cliente == null)
                return NotFound();

            await _clienteRepository.DeleteCliente(cliente);
            return NoContent();
        }
        #endregion
    }
}
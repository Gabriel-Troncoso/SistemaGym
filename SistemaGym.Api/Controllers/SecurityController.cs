using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaGym.Api.Responses;
using SistemaGym.Core.DTOs;
using SistemaGym.Core.Entities;
using SistemaGym.Core.Enum;
using SistemaGym.Services.Interfaces;

namespace SistemaGym.Api.Controllers
{
    [Authorize(Roles = nameof(RoleType.Administrator))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityService _securityService;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;

        public SecurityController(
            ISecurityService securityService,
            IMapper mapper,
            IPasswordService passwordService)
        {
            _securityService = securityService;
            _mapper = mapper;
            _passwordService = passwordService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(SecurityDto securityDto)
        {
            var usuario = _mapper.Map<Usuario>(securityDto);
            usuario.Password = _passwordService.Hash(securityDto.Password);
            usuario.Rol = securityDto.Role?.ToString() ?? RoleType.Consumer.ToString();

            await _securityService.RegisterUser(usuario);

            securityDto = _mapper.Map<SecurityDto>(usuario);
            var response = new ApiResponse<SecurityDto>(securityDto);

            return Ok(response);
        }
    }
}

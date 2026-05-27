using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SistemaGym.Core.Entities;
using SistemaGym.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SistemaGym.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ISecurityService _securityService;
        private readonly IPasswordService _passwordService;

        public TokenController(
            IConfiguration configuration,
            ISecurityService securityService,
            IPasswordService passwordService)
        {
            _configuration = configuration;
            _securityService = securityService;
            _passwordService = passwordService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            var validation = await IsValidUser(userLogin);

            if (validation.Item1)
            {
                var token = GenerateToken(validation.Item2!);
                return Ok(new { token });
            }

            return Unauthorized();
        }

        private async Task<(bool, Usuario?)> IsValidUser(UserLogin userLogin)
        {
            var user = await _securityService.GetLoginByCredentials(userLogin);

            if (user == null ||
                string.IsNullOrWhiteSpace(user.Password) ||
                string.IsNullOrWhiteSpace(user.Email))
            {
                return (false, user);
            }

            var isValid = _passwordService.Check(user.Password, userLogin.Password);
            return (isValid, user);
        }

        private string GenerateToken(Usuario usuario)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Authentication:SecretKey"] ?? string.Empty));

            var signingCredentials = new SigningCredentials(
                symmetricSecurityKey,
                SecurityAlgorithms.HmacSha256);

            var header = new JwtHeader(signingCredentials);

            var claims = new[]
            {
                new Claim("Name", usuario.Nombre ?? string.Empty),
                new Claim("Login", usuario.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, usuario.Rol ?? string.Empty)
            };

            var payload = new JwtPayload(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(60));

            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

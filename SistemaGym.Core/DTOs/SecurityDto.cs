using SistemaGym.Core.Enum;

namespace SistemaGym.Core.DTOs
{
    public class SecurityDto
    {
        public string Login { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public RoleType? Role { get; set; }
    }
}

namespace SistemaGym.Core.Entities
{
    public class Usuario : BaseEntity
    {
        public string? Nombre { get; set; }

        public string? Telefono { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Rol { get; set; }

        public bool? Estado { get; set; }

        public DateTime? FechaRegistro { get; set; }
    }
}
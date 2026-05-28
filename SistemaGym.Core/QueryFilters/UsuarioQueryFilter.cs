namespace SistemaGym.Core.QueryFilters
{
    public class UsuarioQueryFilter : PaginationQueryFilter
    {
        public string? Nombre { get; set; }

        public string? Telefono { get; set; }

        public string? Email { get; set; }

        public string? Rol { get; set; }

        public bool? Estado { get; set; }
    }
}

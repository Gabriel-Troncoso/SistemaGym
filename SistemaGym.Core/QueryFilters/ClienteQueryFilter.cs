namespace SistemaGym.Core.QueryFilters
{
    public class ClienteQueryFilter : PaginationQueryFilter
    {
        public string? Nombre { get; set; }

        public string? Apellido { get; set; }

        public string? Ci { get; set; }

        public string? Correo { get; set; }

        public string? Telefono { get; set; }
    }
}

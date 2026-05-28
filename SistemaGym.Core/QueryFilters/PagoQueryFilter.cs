namespace SistemaGym.Core.QueryFilters
{
    public class PagoQueryFilter : PaginationQueryFilter
    {
        public int? MembresiaId { get; set; }

        public decimal? MontoMin { get; set; }

        public decimal? MontoMax { get; set; }

        public DateTime? FechaPagoDesde { get; set; }

        public DateTime? FechaPagoHasta { get; set; }

        public string? MetodoPago { get; set; }

        public bool? Estado { get; set; }
    }
}

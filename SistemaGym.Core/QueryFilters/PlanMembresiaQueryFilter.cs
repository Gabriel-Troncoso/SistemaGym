namespace SistemaGym.Core.QueryFilters
{
    public class PlanMembresiaQueryFilter : PaginationQueryFilter
    {
        public string? NombrePlan { get; set; }

        public int? DuracionDias { get; set; }

        public decimal? PrecioMin { get; set; }

        public decimal? PrecioMax { get; set; }

        public bool? Estado { get; set; }
    }
}

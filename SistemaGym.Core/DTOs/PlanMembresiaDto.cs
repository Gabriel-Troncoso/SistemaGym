namespace SistemaGym.Core.DTOs
{
    public class PlanMembresiaDto
    {
        public int Id { get; set; }

        public string? NombrePlan { get; set; }

        public string? Descripcion { get; set; }

        public int? DuracionDias { get; set; }

        public decimal? Precio { get; set; }

        public bool? Estado { get; set; }
    }
}
namespace SistemaGym.Core.Entities
{
    public class PlanMembresia : BaseEntity
    {
        public string? NombrePlan { get; set; }

        public string? Descripcion { get; set; }

        public int? DuracionDias { get; set; }

        public decimal? Precio { get; set; }

        public bool? Estado { get; set; }

        public ICollection<Membresia>? Membresias { get; set; }
    }
}
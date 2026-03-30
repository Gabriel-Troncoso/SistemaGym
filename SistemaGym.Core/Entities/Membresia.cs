namespace SistemaGym.Core.Entities
{
    public class Membresia : BaseEntity
    {
        public int ClienteId { get; set; }

        public int PlanMembresiaId { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public bool? Estado { get; set; }

        public Cliente? Cliente { get; set; }

        public PlanMembresia? PlanMembresia { get; set; }

        public ICollection<Pago>? Pagos { get; set; }
    }
}
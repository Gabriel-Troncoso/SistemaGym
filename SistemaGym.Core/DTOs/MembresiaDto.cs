namespace SistemaGym.Core.DTOs
{
    public class MembresiaDto
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }

        public int PlanMembresiaId { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public bool? Estado { get; set; }
    }
}
namespace SistemaGym.Core.DTOs
{
    public class PagoDto
    {
        public int Id { get; set; }

        public int MembresiaId { get; set; }

        public decimal? Monto { get; set; }

        public DateTime? FechaPago { get; set; }

        public string? MetodoPago { get; set; }

        public bool? Estado { get; set; }
    }
}
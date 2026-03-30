namespace SistemaGym.Core.Entities
{
    public class Pago : BaseEntity
    {
        public int MembresiaId { get; set; }

        public decimal? Monto { get; set; }

        public DateTime? FechaPago { get; set; }

        public string? MetodoPago { get; set; }

        public bool? Estado { get; set; }

        public Membresia? Membresia { get; set; }
    }
}
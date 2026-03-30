using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaGym.Core.Entities;

namespace SistemaGym.Infrastructure.Data.Configurations
{
    public class PagoConfiguration : IEntityTypeConfiguration<Pago>
    {
        public void Configure(EntityTypeBuilder<Pago> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_Pago");

            builder.ToTable("pago");

            builder.Property(e => e.Id)
                .HasColumnName("idPago");

            builder.Property(e => e.Monto)
                .HasColumnType("decimal(10,2)");

            builder.Property(e => e.FechaPago)
                .HasColumnType("datetime");

            builder.Property(e => e.MetodoPago)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.HasOne(d => d.Membresia)
                .WithMany(p => p.Pagos)
                .HasForeignKey(d => d.MembresiaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pago_Membresia");
        }
    }
}
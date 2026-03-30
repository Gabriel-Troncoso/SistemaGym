using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaGym.Core.Entities;

namespace SistemaGym.Infrastructure.Data.Configurations
{
    public class MembresiaConfiguration : IEntityTypeConfiguration<Membresia>
    {
        public void Configure(EntityTypeBuilder<Membresia> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_Membresia");

            builder.ToTable("membresia");

            builder.Property(e => e.Id)
                .HasColumnName("idMembresia");

            builder.Property(e => e.FechaInicio)
                .HasColumnType("datetime");

            builder.Property(e => e.FechaFin)
                .HasColumnType("datetime");

            builder.HasOne(d => d.Cliente)
                .WithMany(p => p.Membresias)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Membresia_Cliente");

            builder.HasOne(d => d.PlanMembresia)
                .WithMany(p => p.Membresias)
                .HasForeignKey(d => d.PlanMembresiaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Membresia_PlanMembresia");
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaGym.Core.Entities;

namespace SistemaGym.Infrastructure.Data.Configurations
{
    public class PlanMembresiaConfiguration : IEntityTypeConfiguration<PlanMembresia>
    {
        public void Configure(EntityTypeBuilder<PlanMembresia> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_PlanMembresia");

            builder.ToTable("plan_membresia");

            builder.Property(e => e.Id)
                .HasColumnName("idPlan");

            builder.Property(e => e.NombrePlan)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .IsUnicode(false);

            builder.Property(e => e.Precio)
                .HasColumnType("decimal(10,2)");
        }
    }
}
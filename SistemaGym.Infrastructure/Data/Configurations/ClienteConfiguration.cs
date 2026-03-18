using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaGym.Core.Entities;

namespace SistemaGym.Infrastructure.Data.Configurations
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_Cliente");

            builder.ToTable("cliente");

            builder.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.Apellido)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.Ci)
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.Correo)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.FechaRegistro)
                .HasColumnType("datetime");
        }
    }
}
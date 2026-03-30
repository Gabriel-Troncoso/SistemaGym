using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaGym.Core.Entities;

namespace SistemaGym.Infrastructure.Data.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_Usuario");

            builder.ToTable("usuario");

            builder.Property(e => e.Id)
                .HasColumnName("idUsuario");

            builder.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.Rol)
                .HasMaxLength(30)
                .IsUnicode(false);

            builder.Property(e => e.FechaRegistro)
                .HasColumnType("datetime");
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class CtUsuarioCfg : IEntityTypeConfiguration<CtUsuarioEtt>
{
    public void Configure(EntityTypeBuilder<CtUsuarioEtt> builder)
    {
        builder.ToTable("CtUsuario");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("smallint")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(e => e.IdCtRol)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.Nombre)
            .HasColumnType("varchar(25)")
            .HasMaxLength(25)
            .IsRequired();
        builder.Property(e => e.PrimerApellido)
            .HasColumnType("varchar(25)")
            .HasMaxLength(25)
            .IsRequired();
        builder.Property(e => e.SegundoApellido)
            .HasColumnType("varchar(25)")
            .HasMaxLength(25)
            .IsRequired();
        builder.Property(e => e.Direccion)
            .HasColumnType("varchar(250)")
            .HasMaxLength(250)
            .IsRequired();
        builder.Property(e => e.Telefono)
            .HasColumnType("varchar(10)")
            .HasMaxLength(10)
            .IsRequired();
        builder.Property(e => e.Usuario)
            .HasColumnType("varchar(8)")
            .HasMaxLength(8)
            .IsRequired();
        builder.Property(e => e.Contrasenia)
            .HasColumnType("varchar(100)")
            .HasMaxLength(100);
        builder.Property(e => e.Activo)
            .HasColumnType("bit")
            .IsRequired();

        builder.HasOne(e => e.Nav_IdCtRol)
            .WithMany(p => p.Coll_CtUsuario_IdCtRol)
            .HasForeignKey(e => e.IdCtRol)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
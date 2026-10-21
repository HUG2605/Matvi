using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class CtProveedorCfg : IEntityTypeConfiguration<CtProveedorEtt>
{
    public void Configure(EntityTypeBuilder<CtProveedorEtt> builder)
    {
        builder.ToTable("CtProveedor");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(e => e.Nombre)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(e => e.Direccion)
            .HasColumnType("varchar(250)")
            .HasMaxLength(250)
            .IsRequired();
        builder.Property(e => e.Telefono)
            .HasColumnType("varchar(10)")
            .HasMaxLength(10)
            .IsRequired();
        builder.Property(e => e.NombreContacto)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(e => e.TelefonoContacto)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired();
    }
}
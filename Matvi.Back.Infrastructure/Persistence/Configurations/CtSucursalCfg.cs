using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class CtSucursalCfg : IEntityTypeConfiguration<CtSucursalEtt>
{
    public void Configure(EntityTypeBuilder<CtSucursalEtt> builder)
    {
        builder.ToTable("CtSucursal");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("tinyint")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(e => e.IdCtEmpresa)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.Nombre)
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .IsRequired();
        builder.Property(e => e.Clave)
            .HasColumnType("char(4)")
            .HasMaxLength(4)
            .IsRequired();
        builder.Property(e => e.Direccion)
            .HasColumnType("varchar(250)")
            .HasMaxLength(250)
            .IsRequired();
        builder.Property(e => e.Telefono)
            .HasColumnType("varchar(10)")
            .HasMaxLength(10)
            .IsRequired();

        builder.HasOne(e => e.Nav_IdCtEmpresa)
            .WithMany(p => p.Coll_CtSucursal_IdCtEmpresa)
            .HasForeignKey(e => e.IdCtEmpresa)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class RlProductoSucursalCfg : IEntityTypeConfiguration<RlProductoSucursalEtt>
{
    public void Configure(EntityTypeBuilder<RlProductoSucursalEtt> builder)
    {
        builder.ToTable("RlProductoSucursal");

        builder.HasKey(e => new { e.IdCtProducto, e.IdCtSucursal });

        builder.Property(e => e.IdCtProducto)
            .HasColumnType("int")
            .IsRequired();
        builder.Property(e => e.IdCtSucursal)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.StockMinimoUBI)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();
        builder.Property(e => e.StockMaximoUBI)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();
        builder.Property(e => e.Regularizado)
            .HasColumnType("bit")
            .IsRequired();
        builder.Property(e => e.Ubicacion)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50);
        builder.Property(e => e.StockReservadoUBI)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();

        builder.HasOne(e => e.Nav_IdCtSucursal)
            .WithMany(p => p.Coll_RlProductoSucursal_IdCtSucursal)
            .HasForeignKey(e => e.IdCtSucursal)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtProducto)
            .WithMany(p => p.Coll_RlProductoSucursal_IdCtProducto)
            .HasForeignKey(e => e.IdCtProducto)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
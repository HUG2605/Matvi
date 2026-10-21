using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class OpMovimientoInventarioCfg : IEntityTypeConfiguration<OpMovimientoInventarioEtt>
{
    public void Configure(EntityTypeBuilder<OpMovimientoInventarioEtt> builder)
    {
        builder.ToTable("OpMovimientoInventario");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(e => e.IdCtSucursal)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.Numero)
            .HasColumnType("int")
            .IsRequired();
        builder.Property(e => e.Cpp_IdCtProducto)
            .HasColumnType("int")
            .IsRequired();
        builder.Property(e => e.Cpp_Nombre)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(e => e.Cpp_FactorUBI)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();
        builder.Property(e => e.CantidadUBI)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();
        builder.Property(e => e.Fecha)
            .HasColumnType("datetime")
            .IsRequired();
        builder.Property(e => e.IdCtTipoMovimiento)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.IdOpPedidoDetalleEvento)
            .HasColumnType("int");
        builder.Property(e => e.IdOpCompraDetalle)
            .HasColumnType("int");
        builder.Property(e => e.IdOpTraspasoDetalle)
            .HasColumnType("int");
        builder.Property(e => e.IdOpLote)
            .HasColumnType("int")
            .IsRequired();
        builder.Property(e => e.CostoUBI)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5);
        builder.Property(e => e.CostoTotal)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5);
        builder.Property(e => e.IdCtUsuario)
            .HasColumnType("smallint")
            .IsRequired();

        builder.HasOne(e => e.Nav_IdCtSucursal)
            .WithMany(p => p.Coll_OpMovimientoInventario_IdCtSucursal)
            .HasForeignKey(e => e.IdCtSucursal)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdOpPedidoDetalleEvento)
            .WithMany(p => p.Coll_OpMovimientoInventario_IdOpPedidoDetalleEvento)
            .HasForeignKey(e => e.IdOpPedidoDetalleEvento)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdOpCompraDetalle)
            .WithMany(p => p.Coll_OpMovimientoInventario_IdOpCompraDetalle)
            .HasForeignKey(e => e.IdOpCompraDetalle)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdOpLote)
            .WithMany(p => p.Coll_OpMovimientoInventario_IdOpLote)
            .HasForeignKey(e => e.IdOpLote)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdOpTraspasoDetalle)
            .WithMany(p => p.Coll_OpMovimientoInventario_IdOpTraspasoDetalle)
            .HasForeignKey(e => e.IdOpTraspasoDetalle)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtUsuario)
            .WithMany(p => p.Coll_OpMovimientoInventario_IdCtUsuario)
            .HasForeignKey(e => e.IdCtUsuario)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtTipoMovimiento)
            .WithMany(p => p.Coll_OpMovimientoInventario_IdCtTipoMovimiento)
            .HasForeignKey(e => e.IdCtTipoMovimiento)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_Cpp_IdCtProducto)
            .WithMany(p => p.Coll_OpMovimientoInventario_Cpp_IdCtProducto)
            .HasForeignKey(e => e.Cpp_IdCtProducto)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
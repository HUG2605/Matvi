using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class OpPedidoDetalleCfg : IEntityTypeConfiguration<OpPedidoDetalleEtt>
{
    public void Configure(EntityTypeBuilder<OpPedidoDetalleEtt> builder)
    {
        builder.ToTable("OpPedidoDetalle");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(e => e.IdOpPedido)
            .HasColumnType("int")
            .IsRequired();
        builder.Property(e => e.IdCtEstatusPedidoDetalle)
            .HasColumnType("tinyint")
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
        builder.Property(e => e.Cantidad)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();
        builder.Property(e => e.CantidadSurtida)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();
        builder.Property(e => e.CantidadEntregada)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();
        builder.Property(e => e.CantidadRechazada)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();
        builder.Property(e => e.CantidadCancelada)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();
        builder.Property(e => e.Precio)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();
        builder.Property(e => e.Importe)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();
        builder.Property(e => e.IdOpMovimientoInventario_Salida)
            .HasColumnType("int");
        builder.Property(e => e.IdOpMovimientoInventario_Reingreso)
            .HasColumnType("int");

        builder.HasOne(e => e.Nav_Cpp_IdCtProducto)
            .WithMany(p => p.Coll_OpPedidoDetalle_Cpp_IdCtProducto)
            .HasForeignKey(e => e.Cpp_IdCtProducto)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtEstatusPedidoDetalle)
            .WithMany(p => p.Coll_OpPedidoDetalle_IdCtEstatusPedidoDetalle)
            .HasForeignKey(e => e.IdCtEstatusPedidoDetalle)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdOpPedido)
            .WithMany(p => p.Coll_OpPedidoDetalle_IdOpPedido)
            .HasForeignKey(e => e.IdOpPedido)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
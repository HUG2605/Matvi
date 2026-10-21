using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class OpCompraDetalleCfg : IEntityTypeConfiguration<OpCompraDetalleEtt>
{
    public void Configure(EntityTypeBuilder<OpCompraDetalleEtt> builder)
    {
        builder.ToTable("OpCompraDetalle");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(e => e.IdOpCompra)
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
        builder.Property(e => e.Cantidad)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();
        builder.Property(e => e.CostoUP)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();
        builder.Property(e => e.CostoUBI)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();
        builder.Property(e => e.Importe)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();
        builder.Property(e => e.IdOpLote)
            .HasColumnType("int")
            .IsRequired();

        builder.HasOne(e => e.Nav_Cpp_IdCtProducto)
            .WithMany(p => p.Coll_OpCompraDetalle_Cpp_IdCtProducto)
            .HasForeignKey(e => e.Cpp_IdCtProducto)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdOpCompra)
            .WithMany(p => p.Coll_OpCompraDetalle_IdOpCompra)
            .HasForeignKey(e => e.IdOpCompra)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdOpLote)
            .WithMany(p => p.Coll_OpCompraDetalle_IdOpLote)
            .HasForeignKey(e => e.IdOpLote)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class OpPagoDetalleCfg : IEntityTypeConfiguration<OpPagoDetalleEtt>
{
    public void Configure(EntityTypeBuilder<OpPagoDetalleEtt> builder)
    {
        builder.ToTable("OpPagoDetalle");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(e => e.IdOpPago)
            .HasColumnType("int")
            .IsRequired();
        builder.Property(e => e.IdOpCompra)
            .HasColumnType("int")
            .IsRequired();
        builder.Property(e => e.MontoAplicado)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();

        builder.HasOne(e => e.Nav_IdOpCompra)
            .WithMany(p => p.Coll_OpPagoDetalle_IdOpCompra)
            .HasForeignKey(e => e.IdOpCompra)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdOpPago)
            .WithMany(p => p.Coll_OpPagoDetalle_IdOpPago)
            .HasForeignKey(e => e.IdOpPago)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
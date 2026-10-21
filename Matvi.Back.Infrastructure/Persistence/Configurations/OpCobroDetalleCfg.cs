using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class OpCobroDetalleCfg : IEntityTypeConfiguration<OpCobroDetalleEtt>
{
    public void Configure(EntityTypeBuilder<OpCobroDetalleEtt> builder)
    {
        builder.ToTable("OpCobroDetalle");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(e => e.IdOpCobro)
            .HasColumnType("int")
            .IsRequired();
        builder.Property(e => e.IdOpPedido)
            .HasColumnType("int")
            .IsRequired();
        builder.Property(e => e.MontoAplicado)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();

        builder.HasOne(e => e.Nav_IdOpPedido)
            .WithMany(p => p.Coll_OpCobroDetalle_IdOpPedido)
            .HasForeignKey(e => e.IdOpPedido)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdOpCobro)
            .WithMany(p => p.Coll_OpCobroDetalle_IdOpCobro)
            .HasForeignKey(e => e.IdOpCobro)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
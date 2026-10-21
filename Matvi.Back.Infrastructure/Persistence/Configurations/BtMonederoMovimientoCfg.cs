using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class BtMonederoMovimientoCfg : IEntityTypeConfiguration<BtMonederoMovimientoEtt>
{
    public void Configure(EntityTypeBuilder<BtMonederoMovimientoEtt> builder)
    {
        builder.ToTable("BtMonederoMovimiento");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(e => e.IdCtCliente)
            .HasColumnType("int")
            .IsRequired();
        builder.Property(e => e.IdOpPedido)
            .HasColumnType("int");
        builder.Property(e => e.Tipo)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.Monto)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();
        builder.Property(e => e.SaldoRestante)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();
        builder.Property(e => e.Fecha)
            .HasColumnType("datetime")
            .IsRequired();

        builder.HasOne(e => e.Nav_IdCtCliente)
            .WithMany(p => p.Coll_BtMonederoMovimiento_IdCtCliente)
            .HasForeignKey(e => e.IdCtCliente)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdOpPedido)
            .WithMany(p => p.Coll_BtMonederoMovimiento_IdOpPedido)
            .HasForeignKey(e => e.IdOpPedido)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
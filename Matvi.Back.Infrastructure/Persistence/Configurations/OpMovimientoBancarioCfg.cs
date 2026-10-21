using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class OpMovimientoBancarioCfg : IEntityTypeConfiguration<OpMovimientoBancarioEtt>
{
    public void Configure(EntityTypeBuilder<OpMovimientoBancarioEtt> builder)
    {
        builder.ToTable("OpMovimientoBancario");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(e => e.IdCtCuentaBancaria)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.Fecha)
            .HasColumnType("datetime")
            .IsRequired();
        builder.Property(e => e.Tipo)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.Monto)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2)
            .IsRequired();
        builder.Property(e => e.Referencia)
            .HasColumnType("varchar(100)")
            .HasMaxLength(100);
        builder.Property(e => e.Conciliado)
            .HasColumnType("bit")
            .IsRequired();
        builder.Property(e => e.IdOpCobro)
            .HasColumnType("int");
        builder.Property(e => e.IdOpGasto)
            .HasColumnType("int");
        builder.Property(e => e.IdOpPago)
            .HasColumnType("int");

        builder.HasOne(e => e.Nav_IdOpPago)
            .WithMany(p => p.Coll_OpMovimientoBancario_IdOpPago)
            .HasForeignKey(e => e.IdOpPago)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtCuentaBancaria)
            .WithMany(p => p.Coll_OpMovimientoBancario_IdCtCuentaBancaria)
            .HasForeignKey(e => e.IdCtCuentaBancaria)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdOpCobro)
            .WithMany(p => p.Coll_OpMovimientoBancario_IdOpCobro)
            .HasForeignKey(e => e.IdOpCobro)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdOpGasto)
            .WithMany(p => p.Coll_OpMovimientoBancario_IdOpGasto)
            .HasForeignKey(e => e.IdOpGasto)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
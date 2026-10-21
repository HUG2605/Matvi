using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class OpCobroCfg : IEntityTypeConfiguration<OpCobroEtt>
{
    public void Configure(EntityTypeBuilder<OpCobroEtt> builder)
    {
        builder.ToTable("OpCobro");

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
        builder.Property(e => e.Fecha)
            .HasColumnType("datetime")
            .IsRequired();
        builder.Property(e => e.IdCtCliente)
            .HasColumnType("int")
            .IsRequired();
        builder.Property(e => e.MontoTotal)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();
        builder.Property(e => e.IdCtMedioPago)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.IdCtUsuarioRecibe)
            .HasColumnType("smallint")
            .IsRequired();
        builder.Property(e => e.Notas)
            .HasColumnType("varchar(500)")
            .HasMaxLength(500);
        builder.Property(e => e.IdOpCorteCaja)
            .HasColumnType("int");

        builder.HasOne(e => e.Nav_IdCtSucursal)
            .WithMany(p => p.Coll_OpCobro_IdCtSucursal)
            .HasForeignKey(e => e.IdCtSucursal)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtMedioPago)
            .WithMany(p => p.Coll_OpCobro_IdCtMedioPago)
            .HasForeignKey(e => e.IdCtMedioPago)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtCliente)
            .WithMany(p => p.Coll_OpCobro_IdCtCliente)
            .HasForeignKey(e => e.IdCtCliente)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtUsuarioRecibe)
            .WithMany(p => p.Coll_OpCobro_IdCtUsuarioRecibe)
            .HasForeignKey(e => e.IdCtUsuarioRecibe)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdOpCorteCaja)
            .WithMany(p => p.Coll_OpCobro_IdOpCorteCaja)
            .HasForeignKey(e => e.IdOpCorteCaja)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
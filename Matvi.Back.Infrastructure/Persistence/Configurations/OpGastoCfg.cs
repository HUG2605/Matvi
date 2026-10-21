using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class OpGastoCfg : IEntityTypeConfiguration<OpGastoEtt>
{
    public void Configure(EntityTypeBuilder<OpGastoEtt> builder)
    {
        builder.ToTable("OpGasto");

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
        builder.Property(e => e.FechaPago)
            .HasColumnType("datetime")
            .IsRequired();
        builder.Property(e => e.IdCtTipoGasto)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.Proveedor)
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(e => e.Concepto)
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(e => e.Importe)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();
        builder.Property(e => e.IdCtMedioPago)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.IdCtUsuario)
            .HasColumnType("smallint")
            .IsRequired();
        builder.Property(e => e.IdOpCorteCaja)
            .HasColumnType("int");
        builder.Property(e => e.Notas)
            .HasColumnType("varchar(500)")
            .HasMaxLength(500);

        builder.HasOne(e => e.Nav_IdCtSucursal)
            .WithMany(p => p.Coll_OpGasto_IdCtSucursal)
            .HasForeignKey(e => e.IdCtSucursal)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdOpCorteCaja)
            .WithMany(p => p.Coll_OpGasto_IdOpCorteCaja)
            .HasForeignKey(e => e.IdOpCorteCaja)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtMedioPago)
            .WithMany(p => p.Coll_OpGasto_IdCtMedioPago)
            .HasForeignKey(e => e.IdCtMedioPago)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtTipoGasto)
            .WithMany(p => p.Coll_OpGasto_IdCtTipoGasto)
            .HasForeignKey(e => e.IdCtTipoGasto)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtUsuario)
            .WithMany(p => p.Coll_OpGasto_IdCtUsuario)
            .HasForeignKey(e => e.IdCtUsuario)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
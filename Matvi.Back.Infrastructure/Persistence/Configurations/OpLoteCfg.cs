using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class OpLoteCfg : IEntityTypeConfiguration<OpLoteEtt>
{
    public void Configure(EntityTypeBuilder<OpLoteEtt> builder)
    {
        builder.ToTable("OpLote");

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
        builder.Property(e => e.IdCtTipoLote)
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
        builder.Property(e => e.CantidadInicialUBI)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();
        builder.Property(e => e.CantidadDisponibleUBI)
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
        builder.Property(e => e.Fecha)
            .HasColumnType("datetime")
            .IsRequired();

        builder.HasOne(e => e.Nav_IdCtSucursal)
            .WithMany(p => p.Coll_OpLote_IdCtSucursal)
            .HasForeignKey(e => e.IdCtSucursal)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtTipoLote)
            .WithMany(p => p.Coll_OpLote_IdCtTipoLote)
            .HasForeignKey(e => e.IdCtTipoLote)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_Cpp_IdCtProducto)
            .WithMany(p => p.Coll_OpLote_Cpp_IdCtProducto)
            .HasForeignKey(e => e.Cpp_IdCtProducto)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
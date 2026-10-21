using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class OpCompraCfg : IEntityTypeConfiguration<OpCompraEtt>
{
    public void Configure(EntityTypeBuilder<OpCompraEtt> builder)
    {
        builder.ToTable("OpCompra");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(e => e.Fecha)
            .HasColumnType("datetime")
            .IsRequired();
        builder.Property(e => e.IdCtSucursal)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.Numero)
            .HasColumnType("int")
            .IsRequired();
        builder.Property(e => e.IdCtProveedor)
            .HasColumnType("int")
            .IsRequired();
        builder.Property(e => e.Total)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();
        builder.Property(e => e.TotalPagado)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();

        builder.HasOne(e => e.Nav_IdCtSucursal)
            .WithMany(p => p.Coll_OpCompra_IdCtSucursal)
            .HasForeignKey(e => e.IdCtSucursal)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtProveedor)
            .WithMany(p => p.Coll_OpCompra_IdCtProveedor)
            .HasForeignKey(e => e.IdCtProveedor)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
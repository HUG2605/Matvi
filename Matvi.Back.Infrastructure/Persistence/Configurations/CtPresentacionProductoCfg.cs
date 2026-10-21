using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class CtPresentacionProductoCfg : IEntityTypeConfiguration<CtPresentacionProductoEtt>
{
    public void Configure(EntityTypeBuilder<CtPresentacionProductoEtt> builder)
    {
        builder.ToTable("CtPresentacionProducto");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(e => e.IdCtProducto)
            .HasColumnType("int")
            .IsRequired();
        builder.Property(e => e.Nombre)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(e => e.FactorUBI)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();
        builder.Property(e => e.EsParaCompra)
            .HasColumnType("bit")
            .IsRequired();
        builder.Property(e => e.EsParaVenta)
            .HasColumnType("bit")
            .IsRequired();
        builder.Property(e => e.PrecioVenta)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();
        builder.Property(e => e.PrecioMayoreo)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5);
        builder.Property(e => e.UnidadesMayoreo)
            .HasColumnType("smallint");

        builder.HasOne(e => e.Nav_IdCtProducto)
            .WithMany(p => p.Coll_CtPresentacionProducto_IdCtProducto)
            .HasForeignKey(e => e.IdCtProducto)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
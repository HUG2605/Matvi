using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class RlProductoProveedorCfg : IEntityTypeConfiguration<RlProductoProveedorEtt>
{
    public void Configure(EntityTypeBuilder<RlProductoProveedorEtt> builder)
    {
        builder.ToTable("RlProductoProveedor");

        builder.HasKey(e => new { e.IdCtProveedor, e.IdCtProducto });

        builder.Property(e => e.IdCtProducto)
            .HasColumnType("int")
            .IsRequired();
        builder.Property(e => e.IdCtProveedor)
            .HasColumnType("int")
            .IsRequired();

        builder.HasOne(e => e.Nav_IdCtProveedor)
            .WithMany(p => p.Coll_RlProductoProveedor_IdCtProveedor)
            .HasForeignKey(e => e.IdCtProveedor)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtProducto)
            .WithMany(p => p.Coll_RlProductoProveedor_IdCtProducto)
            .HasForeignKey(e => e.IdCtProducto)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
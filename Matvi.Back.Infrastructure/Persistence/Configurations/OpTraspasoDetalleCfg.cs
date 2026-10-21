using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class OpTraspasoDetalleCfg : IEntityTypeConfiguration<OpTraspasoDetalleEtt>
{
    public void Configure(EntityTypeBuilder<OpTraspasoDetalleEtt> builder)
    {
        builder.ToTable("OpTraspasoDetalle");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(e => e.IdOpTraspaso)
            .HasColumnType("int")
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
        builder.Property(e => e.Cantidad)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();
        builder.Property(e => e.IdOpLote_Destino)
            .HasColumnType("int");

        builder.HasOne(e => e.Nav_IdOpLote_Destino)
            .WithMany(p => p.Coll_OpTraspasoDetalle_IdOpLote_Destino)
            .HasForeignKey(e => e.IdOpLote_Destino)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdOpTraspaso)
            .WithMany(p => p.Coll_OpTraspasoDetalle_IdOpTraspaso)
            .HasForeignKey(e => e.IdOpTraspaso)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_Cpp_IdCtProducto)
            .WithMany(p => p.Coll_OpTraspasoDetalle_Cpp_IdCtProducto)
            .HasForeignKey(e => e.Cpp_IdCtProducto)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
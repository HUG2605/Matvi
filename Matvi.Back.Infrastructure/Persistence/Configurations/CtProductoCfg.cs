using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class CtProductoCfg : IEntityTypeConfiguration<CtProductoEtt>
{
    public void Configure(EntityTypeBuilder<CtProductoEtt> builder)
    {
        builder.ToTable("CtProducto");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(e => e.IdCtGrupo)
            .HasColumnType("smallint")
            .IsRequired();
        builder.Property(e => e.Nombre)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(e => e.EspecificacionTecnica)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50);
        builder.Property(e => e.Marca)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50);
        builder.Property(e => e.IdCtUnidadBaseInventario)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.Imagen)
            .HasColumnType("varchar(max)");
        builder.Property(e => e.CodigoBarras)
            .HasColumnType("varchar(25)")
            .HasMaxLength(25);
        builder.Property(e => e.CodigoBarrasInterno)
            .HasColumnType("bit")
            .IsRequired();
        builder.Property(e => e.BajoPedido)
            .HasColumnType("bit")
            .IsRequired();
        builder.Property(e => e.EntregaMostrador)
            .HasColumnType("bit")
            .IsRequired();
        builder.Property(e => e.Activo)
            .HasColumnType("bit")
            .IsRequired();

        builder.HasOne(e => e.Nav_IdCtGrupo)
            .WithMany(p => p.Coll_CtProducto_IdCtGrupo)
            .HasForeignKey(e => e.IdCtGrupo)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtUnidadBaseInventario)
            .WithMany(p => p.Coll_CtProducto_IdCtUnidadBaseInventario)
            .HasForeignKey(e => e.IdCtUnidadBaseInventario)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
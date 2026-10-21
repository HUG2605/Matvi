using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class CtTipoEventoPedidoDetalleCfg : IEntityTypeConfiguration<CtTipoEventoPedidoDetalleEtt>
{
    public void Configure(EntityTypeBuilder<CtTipoEventoPedidoDetalleEtt> builder)
    {
        builder.ToTable("CtTipoEventoPedidoDetalle");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.Nombre)
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .IsRequired();
        builder.Property(e => e.Descripcion)
            .HasColumnType("varchar(300)")
            .HasMaxLength(300)
            .IsRequired();
    }
}
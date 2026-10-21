using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class CtEstatusPedidoDetalleCfg : IEntityTypeConfiguration<CtEstatusPedidoDetalleEtt>
{
    public void Configure(EntityTypeBuilder<CtEstatusPedidoDetalleEtt> builder)
    {
        builder.ToTable("CtEstatusPedidoDetalle");

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
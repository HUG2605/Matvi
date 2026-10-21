using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class CtEstatusPedidoCfg : IEntityTypeConfiguration<CtEstatusPedidoEtt>
{
    public void Configure(EntityTypeBuilder<CtEstatusPedidoEtt> builder)
    {
        builder.ToTable("CtEstatusPedido");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.Nombre)
            .HasColumnType("varchar(25)")
            .HasMaxLength(25)
            .IsRequired();
        builder.Property(e => e.Descripcion)
            .HasColumnType("varchar(300)")
            .HasMaxLength(300)
            .IsRequired();
    }
}
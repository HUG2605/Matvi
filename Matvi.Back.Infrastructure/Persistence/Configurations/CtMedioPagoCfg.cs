using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class CtMedioPagoCfg : IEntityTypeConfiguration<CtMedioPagoEtt>
{
    public void Configure(EntityTypeBuilder<CtMedioPagoEtt> builder)
    {
        builder.ToTable("CtMedioPago");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.Nombre)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired();
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class CtConfiguracionMonederoCfg : IEntityTypeConfiguration<CtConfiguracionMonederoEtt>
{
    public void Configure(EntityTypeBuilder<CtConfiguracionMonederoEtt> builder)
    {
        builder.ToTable("CtConfiguracionMonedero");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("int")
            .IsRequired();
        builder.Property(e => e.PorcentajeAcumulacion)
            .HasColumnType("decimal(6,5)")
            .HasPrecision(6, 5)
            .IsRequired();
        builder.Property(e => e.CostoPremio)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2)
            .IsRequired();
        builder.Property(e => e.NombrePremio)
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(e => e.Activo)
            .HasColumnType("bit")
            .IsRequired();
    }
}
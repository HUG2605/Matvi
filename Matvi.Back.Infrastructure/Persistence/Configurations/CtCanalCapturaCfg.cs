using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class CtCanalCapturaCfg : IEntityTypeConfiguration<CtCanalCapturaEtt>
{
    public void Configure(EntityTypeBuilder<CtCanalCapturaEtt> builder)
    {
        builder.ToTable("CtCanalCaptura");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.Nombre)
            .HasColumnType("varchar(25)")
            .HasMaxLength(25)
            .IsRequired();
    }
}
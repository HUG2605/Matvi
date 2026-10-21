using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class CtUnidadBaseInventarioCfg : IEntityTypeConfiguration<CtUnidadBaseInventarioEtt>
{
    public void Configure(EntityTypeBuilder<CtUnidadBaseInventarioEtt> builder)
    {
        builder.ToTable("CtUnidadBaseInventario");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.Nombre)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(e => e.Abreviatura)
            .HasColumnType("varchar(10)")
            .HasMaxLength(10)
            .IsRequired();
    }
}
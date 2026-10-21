using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class CtModuloCfg : IEntityTypeConfiguration<CtModuloEtt>
{
    public void Configure(EntityTypeBuilder<CtModuloEtt> builder)
    {
        builder.ToTable("CtModulo");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.Nombre)
            .HasColumnType("varchar(25)")
            .HasMaxLength(25)
            .IsRequired();
        builder.Property(e => e.IconName)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(e => e.Area)
            .HasColumnType("varchar(25)")
            .HasMaxLength(25)
            .IsRequired();
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class CtRegimenFiscalCfg : IEntityTypeConfiguration<CtRegimenFiscalEtt>
{
    public void Configure(EntityTypeBuilder<CtRegimenFiscalEtt> builder)
    {
        builder.ToTable("CtRegimenFiscal");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("smallint")
            .IsRequired();
        builder.Property(e => e.Nombre)
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(e => e.Fisica)
            .HasColumnType("bit")
            .IsRequired();
        builder.Property(e => e.Moral)
            .HasColumnType("bit")
            .IsRequired();
    }
}
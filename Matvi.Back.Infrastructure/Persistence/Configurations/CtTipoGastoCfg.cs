using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class CtTipoGastoCfg : IEntityTypeConfiguration<CtTipoGastoEtt>
{
    public void Configure(EntityTypeBuilder<CtTipoGastoEtt> builder)
    {
        builder.ToTable("CtTipoGasto");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("tinyint")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(e => e.Nombre)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired();
    }
}
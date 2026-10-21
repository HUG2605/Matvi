using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class CtSeccionCfg : IEntityTypeConfiguration<CtSeccionEtt>
{
    public void Configure(EntityTypeBuilder<CtSeccionEtt> builder)
    {
        builder.ToTable("CtSeccion");

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
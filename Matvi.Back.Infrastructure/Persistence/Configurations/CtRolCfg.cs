using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class CtRolCfg : IEntityTypeConfiguration<CtRolEtt>
{
    public void Configure(EntityTypeBuilder<CtRolEtt> builder)
    {
        builder.ToTable("CtRol");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.Nombre)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(e => e.Acronimo)
            .HasColumnType("char(3)")
            .HasMaxLength(3)
            .IsRequired();
    }
}
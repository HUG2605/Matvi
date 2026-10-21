using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class CtEmpresaCfg : IEntityTypeConfiguration<CtEmpresaEtt>
{
    public void Configure(EntityTypeBuilder<CtEmpresaEtt> builder)
    {
        builder.ToTable("CtEmpresa");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.Nombre)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(e => e.Logotipo)
            .HasColumnType("varchar(max)");
        builder.Property(e => e.Telefono)
            .HasColumnType("varchar(10)")
            .HasMaxLength(10);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class BtCambioCfg : IEntityTypeConfiguration<BtCambioEtt>
{
    public void Configure(EntityTypeBuilder<BtCambioEtt> builder)
    {
        builder.ToTable("BtCambio");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(e => e.Tabla)
            .HasColumnType("nvarchar(100)")
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(e => e.LlavePrimaria)
            .HasColumnType("nvarchar(100)")
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(e => e.Campo)
            .HasColumnType("nvarchar(100)")
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(e => e.ValorAnterior)
            .HasColumnType("nvarchar(max)");
        builder.Property(e => e.ValorNuevo)
            .HasColumnType("nvarchar(max)");
        builder.Property(e => e.TipoOperacion)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.IdCtUsuario)
            .HasColumnType("smallint")
            .IsRequired();
        builder.Property(e => e.Fecha)
            .HasColumnType("datetime")
            .IsRequired();
    }
}
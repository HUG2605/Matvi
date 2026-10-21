using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class CtCuentaBancariaCfg : IEntityTypeConfiguration<CtCuentaBancariaEtt>
{
    public void Configure(EntityTypeBuilder<CtCuentaBancariaEtt> builder)
    {
        builder.ToTable("CtCuentaBancaria");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.Nombre)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(e => e.Banco)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(e => e.NumeroCuenta)
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .IsRequired();
        builder.Property(e => e.Activa)
            .HasColumnType("bit")
            .IsRequired();
    }
}
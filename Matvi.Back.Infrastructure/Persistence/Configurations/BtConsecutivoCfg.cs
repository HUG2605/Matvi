using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class BtConsecutivoCfg : IEntityTypeConfiguration<BtConsecutivoEtt>
{
    public void Configure(EntityTypeBuilder<BtConsecutivoEtt> builder)
    {
        builder.ToTable("BtConsecutivo");

        builder.HasKey(e => new { e.IdCtSucursal, e.IdCtTipoConsecutivo });

        builder.Property(e => e.IdCtSucursal)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.IdCtTipoConsecutivo)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.UltimoNumero)
            .HasColumnType("int")
            .IsRequired();

        builder.HasOne(e => e.Nav_IdCtSucursal)
            .WithMany(p => p.Coll_BtConsecutivo_IdCtSucursal)
            .HasForeignKey(e => e.IdCtSucursal)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtTipoConsecutivo)
            .WithMany(p => p.Coll_BtConsecutivo_IdCtTipoConsecutivo)
            .HasForeignKey(e => e.IdCtTipoConsecutivo)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
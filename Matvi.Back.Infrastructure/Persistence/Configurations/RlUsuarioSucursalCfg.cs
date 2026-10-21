using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class RlUsuarioSucursalCfg : IEntityTypeConfiguration<RlUsuarioSucursalEtt>
{
    public void Configure(EntityTypeBuilder<RlUsuarioSucursalEtt> builder)
    {
        builder.ToTable("RlUsuarioSucursal");

        builder.HasKey(e => new { e.IdCtUsuario, e.IdCtSucursal });

        builder.Property(e => e.IdCtUsuario)
            .HasColumnType("smallint")
            .IsRequired();
        builder.Property(e => e.IdCtSucursal)
            .HasColumnType("tinyint")
            .IsRequired();

        builder.HasOne(e => e.Nav_IdCtUsuario)
            .WithMany(p => p.Coll_RlUsuarioSucursal_IdCtUsuario)
            .HasForeignKey(e => e.IdCtUsuario)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtSucursal)
            .WithMany(p => p.Coll_RlUsuarioSucursal_IdCtSucursal)
            .HasForeignKey(e => e.IdCtSucursal)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
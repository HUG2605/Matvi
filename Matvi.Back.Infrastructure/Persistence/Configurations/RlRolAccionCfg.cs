using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class RlRolAccionCfg : IEntityTypeConfiguration<RlRolAccionEtt>
{
    public void Configure(EntityTypeBuilder<RlRolAccionEtt> builder)
    {
        builder.ToTable("RlRolAccion");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(e => e.IdCtRol)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.IdCtAccion)
            .HasColumnType("smallint")
            .IsRequired();
        builder.Property(e => e.TieneAcceso)
            .HasColumnType("bit")
            .IsRequired();

        builder.HasOne(e => e.Nav_IdCtRol)
            .WithMany(p => p.Coll_RlRolAccion_IdCtRol)
            .HasForeignKey(e => e.IdCtRol)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtAccion)
            .WithMany(p => p.Coll_RlRolAccion_IdCtAccion)
            .HasForeignKey(e => e.IdCtAccion)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
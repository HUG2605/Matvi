using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class CtAccionCfg : IEntityTypeConfiguration<CtAccionEtt>
{
    public void Configure(EntityTypeBuilder<CtAccionEtt> builder)
    {
        builder.ToTable("CtAccion");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("smallint")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(e => e.IdCtFuncion)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.Nombre)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne(e => e.Nav_IdCtFuncion)
            .WithMany(p => p.Coll_CtAccion_IdCtFuncion)
            .HasForeignKey(e => e.IdCtFuncion)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
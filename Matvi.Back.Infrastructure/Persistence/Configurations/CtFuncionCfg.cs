using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class CtFuncionCfg : IEntityTypeConfiguration<CtFuncionEtt>
{
    public void Configure(EntityTypeBuilder<CtFuncionEtt> builder)
    {
        builder.ToTable("CtFuncion");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.IdCtModulo)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.Nombre)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(e => e.Ruta)
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasOne(e => e.Nav_IdCtModulo)
            .WithMany(p => p.Coll_CtFuncion_IdCtModulo)
            .HasForeignKey(e => e.IdCtModulo)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
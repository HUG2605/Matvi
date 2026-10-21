using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class CtGrupoCfg : IEntityTypeConfiguration<CtGrupoEtt>
{
    public void Configure(EntityTypeBuilder<CtGrupoEtt> builder)
    {
        builder.ToTable("CtGrupo");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("smallint")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(e => e.IdCtSeccion)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.Nombre)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(e => e.Descripcion)
            .HasColumnType("varchar(500)")
            .HasMaxLength(500)
            .IsRequired();

        builder.HasOne(e => e.Nav_IdCtSeccion)
            .WithMany(p => p.Coll_CtGrupo_IdCtSeccion)
            .HasForeignKey(e => e.IdCtSeccion)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
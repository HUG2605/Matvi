using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class RlRolFuncionCfg : IEntityTypeConfiguration<RlRolFuncionEtt>
{
    public void Configure(EntityTypeBuilder<RlRolFuncionEtt> builder)
    {
        builder.ToTable("RlRolFuncion");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(e => e.IdCtRol)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.IdCtFuncion)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.Consultar)
            .HasColumnType("bit")
            .IsRequired();
        builder.Property(e => e.VerSensibles)
            .HasColumnType("bit")
            .IsRequired();

        builder.HasOne(e => e.Nav_IdCtRol)
            .WithMany(p => p.Coll_RlRolFuncion_IdCtRol)
            .HasForeignKey(e => e.IdCtRol)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtFuncion)
            .WithMany(p => p.Coll_RlRolFuncion_IdCtFuncion)
            .HasForeignKey(e => e.IdCtFuncion)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
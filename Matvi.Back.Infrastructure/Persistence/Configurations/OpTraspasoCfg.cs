using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class OpTraspasoCfg : IEntityTypeConfiguration<OpTraspasoEtt>
{
    public void Configure(EntityTypeBuilder<OpTraspasoEtt> builder)
    {
        builder.ToTable("OpTraspaso");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(e => e.IdCtSucursalOrigen)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.IdCtSucursalDestino)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.Fecha)
            .HasColumnType("datetime")
            .IsRequired();
        builder.Property(e => e.IdCtUsuario)
            .HasColumnType("smallint")
            .IsRequired();

        builder.HasOne(e => e.Nav_IdCtSucursalDestino)
            .WithMany(p => p.Coll_OpTraspaso_IdCtSucursalDestino)
            .HasForeignKey(e => e.IdCtSucursalDestino)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtSucursalOrigen)
            .WithMany(p => p.Coll_OpTraspaso_IdCtSucursalOrigen)
            .HasForeignKey(e => e.IdCtSucursalOrigen)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtUsuario)
            .WithMany(p => p.Coll_OpTraspaso_IdCtUsuario)
            .HasForeignKey(e => e.IdCtUsuario)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
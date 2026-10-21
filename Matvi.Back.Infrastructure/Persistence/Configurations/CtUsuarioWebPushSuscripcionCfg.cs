using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class CtUsuarioWebPushSuscripcionCfg : IEntityTypeConfiguration<CtUsuarioWebPushSuscripcionEtt>
{
    public void Configure(EntityTypeBuilder<CtUsuarioWebPushSuscripcionEtt> builder)
    {
        builder.ToTable("CtUsuarioWebPushSuscripcion");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(e => e.IdCtUsuario)
            .HasColumnType("smallint")
            .IsRequired();
        builder.Property(e => e.Endpoint)
            .HasColumnType("varchar(500)")
            .HasMaxLength(500)
            .IsRequired();
        builder.Property(e => e.P256dh)
            .HasColumnType("varchar(500)")
            .HasMaxLength(500)
            .IsRequired();
        builder.Property(e => e.Auth)
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(e => e.FechaRegistro)
            .HasColumnType("datetime")
            .IsRequired();
        builder.Property(e => e.Activa)
            .HasColumnType("bit")
            .IsRequired();

        builder.HasOne(e => e.Nav_IdCtUsuario)
            .WithMany(p => p.Coll_CtUsuarioWebPushSuscripcion_IdCtUsuario)
            .HasForeignKey(e => e.IdCtUsuario)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
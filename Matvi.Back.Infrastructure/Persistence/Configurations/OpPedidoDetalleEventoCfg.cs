using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class OpPedidoDetalleEventoCfg : IEntityTypeConfiguration<OpPedidoDetalleEventoEtt>
{
    public void Configure(EntityTypeBuilder<OpPedidoDetalleEventoEtt> builder)
    {
        builder.ToTable("OpPedidoDetalleEvento");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(e => e.IdOpPedidoDetalle)
            .HasColumnType("int")
            .IsRequired();
        builder.Property(e => e.IdCtTipoEventoPedidoDetalle)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.Cantidad)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();
        builder.Property(e => e.Fecha)
            .HasColumnType("datetime")
            .IsRequired();
        builder.Property(e => e.IdCtUsuario)
            .HasColumnType("smallint")
            .IsRequired();
        builder.Property(e => e.IdCtUsuarioRecibe)
            .HasColumnType("smallint");
        builder.Property(e => e.Notas)
            .HasColumnType("varchar(300)")
            .HasMaxLength(300);

        builder.HasOne(e => e.Nav_IdCtTipoEventoPedidoDetalle)
            .WithMany(p => p.Coll_OpPedidoDetalleEvento_IdCtTipoEventoPedidoDetalle)
            .HasForeignKey(e => e.IdCtTipoEventoPedidoDetalle)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtUsuarioRecibe)
            .WithMany(p => p.Coll_OpPedidoDetalleEvento_IdCtUsuarioRecibe)
            .HasForeignKey(e => e.IdCtUsuarioRecibe)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtUsuario)
            .WithMany(p => p.Coll_OpPedidoDetalleEvento_IdCtUsuario)
            .HasForeignKey(e => e.IdCtUsuario)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdOpPedidoDetalle)
            .WithMany(p => p.Coll_OpPedidoDetalleEvento_IdOpPedidoDetalle)
            .HasForeignKey(e => e.IdOpPedidoDetalle)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
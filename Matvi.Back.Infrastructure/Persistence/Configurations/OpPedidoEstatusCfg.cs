using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class OpPedidoEstatusCfg : IEntityTypeConfiguration<OpPedidoEstatusEtt>
{
    public void Configure(EntityTypeBuilder<OpPedidoEstatusEtt> builder)
    {
        builder.ToTable("OpPedidoEstatus");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(e => e.IdOpPedido)
            .HasColumnType("int")
            .IsRequired();
        builder.Property(e => e.IdCtEstatusPedido)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.Fecha)
            .HasColumnType("datetime")
            .IsRequired();
        builder.Property(e => e.IdCtUsuario)
            .HasColumnType("smallint")
            .IsRequired();
        builder.Property(e => e.Notas)
            .HasColumnType("varchar(250)")
            .HasMaxLength(250)
            .IsRequired();

        builder.HasOne(e => e.Nav_IdCtEstatusPedido)
            .WithMany(p => p.Coll_OpPedidoEstatus_IdCtEstatusPedido)
            .HasForeignKey(e => e.IdCtEstatusPedido)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtUsuario)
            .WithMany(p => p.Coll_OpPedidoEstatus_IdCtUsuario)
            .HasForeignKey(e => e.IdCtUsuario)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdOpPedido)
            .WithMany(p => p.Coll_OpPedidoEstatus_IdOpPedido)
            .HasForeignKey(e => e.IdOpPedido)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
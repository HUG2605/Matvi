using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class OpPedidoCfg : IEntityTypeConfiguration<OpPedidoEtt>
{
    public void Configure(EntityTypeBuilder<OpPedidoEtt> builder)
    {
        builder.ToTable("OpPedido");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(e => e.IdCtSucursal)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.Numero)
            .HasColumnType("int")
            .IsRequired();
        builder.Property(e => e.IdCtCliente)
            .HasColumnType("int")
            .IsRequired();
        builder.Property(e => e.NombreClienteMostrador)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50);
        builder.Property(e => e.TelefonoClienteMostrador)
            .HasColumnType("varchar(10)")
            .HasMaxLength(10);
        builder.Property(e => e.IdCtCanalCaptura)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.IdCtEstatusPedido)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.EntregaDomicilio)
            .HasColumnType("bit")
            .IsRequired();
        builder.Property(e => e.DireccionEntrega)
            .HasColumnType("varchar(250)")
            .HasMaxLength(250);
        builder.Property(e => e.FechaEntregaEstimada)
            .HasColumnType("datetime");
        builder.Property(e => e.Notas)
            .HasColumnType("varchar(250)")
            .HasMaxLength(250)
            .IsRequired();
        builder.Property(e => e.IdCtUsuarioRepartidor)
            .HasColumnType("smallint");
        builder.Property(e => e.Total)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();
        builder.Property(e => e.TotalPagado)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();

        builder.HasOne(e => e.Nav_IdCtSucursal)
            .WithMany(p => p.Coll_OpPedido_IdCtSucursal)
            .HasForeignKey(e => e.IdCtSucursal)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtCanalCaptura)
            .WithMany(p => p.Coll_OpPedido_IdCtCanalCaptura)
            .HasForeignKey(e => e.IdCtCanalCaptura)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtEstatusPedido)
            .WithMany(p => p.Coll_OpPedido_IdCtEstatusPedido)
            .HasForeignKey(e => e.IdCtEstatusPedido)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtCliente)
            .WithMany(p => p.Coll_OpPedido_IdCtCliente)
            .HasForeignKey(e => e.IdCtCliente)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtUsuarioRepartidor)
            .WithMany(p => p.Coll_OpPedido_IdCtUsuarioRepartidor)
            .HasForeignKey(e => e.IdCtUsuarioRepartidor)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
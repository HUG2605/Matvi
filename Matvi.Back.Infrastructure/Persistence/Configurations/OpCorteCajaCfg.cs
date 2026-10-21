using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class OpCorteCajaCfg : IEntityTypeConfiguration<OpCorteCajaEtt>
{
    public void Configure(EntityTypeBuilder<OpCorteCajaEtt> builder)
    {
        builder.ToTable("OpCorteCaja");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(e => e.PorcentajeCoberturaGastos)
            .HasColumnType("decimal(5,2)")
            .HasPrecision(5, 2);
        builder.Property(e => e.PorcentajeVentasSinCostear)
            .HasColumnType("decimal(5,2)")
            .HasPrecision(5, 2);
        builder.Property(e => e.UtilidadNeta)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2);
        builder.Property(e => e.UtilidadBruta)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2);
        builder.Property(e => e.VentasSinCostear)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2);
        builder.Property(e => e.CostoVentasRegularizado)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2);
        builder.Property(e => e.CostoVentasReal)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2);
        builder.Property(e => e.TotalVentas)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2);
        builder.Property(e => e.Diferencia)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2);
        builder.Property(e => e.FondoFinalContado)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2);
        builder.Property(e => e.FondoFinalSistema)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2);
        builder.Property(e => e.TotalPagosBancario)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2);
        builder.Property(e => e.TotalPagosEfectivo)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2);
        builder.Property(e => e.TotalPagos)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2);
        builder.Property(e => e.TotalGastosBancario)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2);
        builder.Property(e => e.TotalGastosEfectivo)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2);
        builder.Property(e => e.TotalGastos)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2);
        builder.Property(e => e.TotalEntregasRepartidor)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2);
        builder.Property(e => e.TotalCobrosBancario)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2);
        builder.Property(e => e.TotalCobrosEfectivo)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2);
        builder.Property(e => e.TotalCobros)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2);
        builder.Property(e => e.FondoInicial)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2)
            .IsRequired();
        builder.Property(e => e.FechaCierre)
            .HasColumnType("datetime");
        builder.Property(e => e.FechaApertura)
            .HasColumnType("datetime")
            .IsRequired();
        builder.Property(e => e.IdCtUsuario)
            .HasColumnType("smallint")
            .IsRequired();
        builder.Property(e => e.Numero)
            .HasColumnType("int")
            .IsRequired();
        builder.Property(e => e.IdCtSucursal)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(e => e.GastosAbsorbidosDeCapital)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2);
        builder.Property(e => e.Notas)
            .HasColumnType("varchar(500)")
            .HasMaxLength(500);

        builder.HasOne(e => e.Nav_IdCtSucursal)
            .WithMany(p => p.Coll_OpCorteCaja_IdCtSucursal)
            .HasForeignKey(e => e.IdCtSucursal)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Nav_IdCtUsuario)
            .WithMany(p => p.Coll_OpCorteCaja_IdCtUsuario)
            .HasForeignKey(e => e.IdCtUsuario)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
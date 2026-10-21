using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Persistence.Configurations;

public class CtClienteCfg : IEntityTypeConfiguration<CtClienteEtt>
{
    public void Configure(EntityTypeBuilder<CtClienteEtt> builder)
    {
        builder.ToTable("CtCliente");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(e => e.EntidadFederativaFiscal)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50);
        builder.Property(e => e.MunicipioFiscal)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50);
        builder.Property(e => e.LocalidadFiscal)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50);
        builder.Property(e => e.ColoniaFiscal)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50);
        builder.Property(e => e.CodigoPostalFiscal)
            .HasColumnType("char(5)")
            .HasMaxLength(5);
        builder.Property(e => e.NumeroInteriorFiscal)
            .HasColumnType("varchar(25)")
            .HasMaxLength(25);
        builder.Property(e => e.SaldoMonedero)
            .HasColumnType("decimal(18,5)")
            .HasPrecision(18, 5)
            .IsRequired();
        builder.Property(e => e.NumeroExteriorFiscal)
            .HasColumnType("varchar(25)")
            .HasMaxLength(25);
        builder.Property(e => e.IdCtRegimenFiscal)
            .HasColumnType("smallint");
        builder.Property(e => e.RFCFiscal)
            .HasColumnType("varchar(13)")
            .HasMaxLength(13);
        builder.Property(e => e.Telefono)
            .HasColumnType("char(10)")
            .HasMaxLength(10)
            .IsRequired();
        builder.Property(e => e.Direccion)
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(e => e.Alias)
            .HasColumnType("varchar(25)")
            .HasMaxLength(25);
        builder.Property(e => e.Nombre)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(e => e.CalleFiscal)
            .HasColumnType("varchar(100)")
            .HasMaxLength(100);
        builder.Property(e => e.LimiteCreditoSemanal)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasOne(e => e.Nav_IdCtRegimenFiscal)
            .WithMany(p => p.Coll_CtCliente_IdCtRegimenFiscal)
            .HasForeignKey(e => e.IdCtRegimenFiscal)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class OpCorteCajaEtt : IAuditable
{
    public int Id { get; set; }
    public byte IdCtSucursal { get; set; }
    public int Numero { get; set; }
    public short IdCtUsuario { get; set; }
    public DateTime FechaApertura { get; set; }
    public DateTime? FechaCierre { get; set; }
    public decimal FondoInicial { get; set; }
    public decimal? TotalCobros { get; set; }
    public decimal? TotalCobrosEfectivo { get; set; }
    public decimal? TotalCobrosBancario { get; set; }
    public decimal? TotalEntregasRepartidor { get; set; }
    public decimal? TotalGastos { get; set; }
    public decimal? TotalGastosEfectivo { get; set; }
    public decimal? TotalGastosBancario { get; set; }
    public decimal? TotalPagos { get; set; }
    public decimal? TotalPagosEfectivo { get; set; }
    public decimal? TotalPagosBancario { get; set; }
    public decimal? FondoFinalSistema { get; set; }
    public decimal? FondoFinalContado { get; set; }
    public decimal? Diferencia { get; set; }
    public decimal? TotalVentas { get; set; }
    public decimal? CostoVentasReal { get; set; }
    public decimal? CostoVentasRegularizado { get; set; }
    public decimal? VentasSinCostear { get; set; }
    public decimal? UtilidadBruta { get; set; }
    public decimal? UtilidadNeta { get; set; }
    public decimal? PorcentajeVentasSinCostear { get; set; }
    public decimal? PorcentajeCoberturaGastos { get; set; }
    public decimal? GastosAbsorbidosDeCapital { get; set; }
    public string? Notas { get; set; }
    public CtSucursalEtt Nav_IdCtSucursal { get; set; } = null!;
    public CtUsuarioEtt Nav_IdCtUsuario { get; set; } = null!;
    public ICollection<OpGastoEtt> Coll_OpGasto_IdOpCorteCaja { get; set; } = new List<OpGastoEtt>();
    public ICollection<OpCobroEtt> Coll_OpCobro_IdOpCorteCaja { get; set; } = new List<OpCobroEtt>();
    public ICollection<OpPagoEtt> Coll_OpPago_IdOpCorteCaja { get; set; } = new List<OpPagoEtt>();
}
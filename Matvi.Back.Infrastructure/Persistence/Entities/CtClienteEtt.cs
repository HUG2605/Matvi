using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class CtClienteEtt : IAuditable
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Alias { get; set; }
    public string Direccion { get; set; } = null!;
    public string Telefono { get; set; } = null!;
    public string? RFCFiscal { get; set; }
    public short? IdCtRegimenFiscal { get; set; }
    public string? CalleFiscal { get; set; }
    public string? NumeroExteriorFiscal { get; set; }
    public string? NumeroInteriorFiscal { get; set; }
    public string? CodigoPostalFiscal { get; set; }
    public string? ColoniaFiscal { get; set; }
    public string? LocalidadFiscal { get; set; }
    public string? MunicipioFiscal { get; set; }
    public string? EntidadFederativaFiscal { get; set; }
    public decimal SaldoMonedero { get; set; }
    public decimal LimiteCreditoSemanal { get; set; }
    public CtRegimenFiscalEtt? Nav_IdCtRegimenFiscal { get; set; }
    public ICollection<BtMonederoMovimientoEtt> Coll_BtMonederoMovimiento_IdCtCliente { get; set; } = new List<BtMonederoMovimientoEtt>();
    public ICollection<OpCobroEtt> Coll_OpCobro_IdCtCliente { get; set; } = new List<OpCobroEtt>();
    public ICollection<OpPedidoEtt> Coll_OpPedido_IdCtCliente { get; set; } = new List<OpPedidoEtt>();
}
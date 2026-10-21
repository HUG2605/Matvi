using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class OpTraspasoDetalleEtt : IAuditable
{
    public int Id { get; set; }
    public int IdOpTraspaso { get; set; }
    public int Cpp_IdCtProducto { get; set; }
    public string Cpp_Nombre { get; set; } = null!;
    public decimal Cpp_FactorUBI { get; set; }
    public decimal Cantidad { get; set; }
    public int? IdOpLote_Destino { get; set; }
    public OpTraspasoEtt Nav_IdOpTraspaso { get; set; } = null!;
    public CtProductoEtt Nav_Cpp_IdCtProducto { get; set; } = null!;
    public OpLoteEtt? Nav_IdOpLote_Destino { get; set; }
    public ICollection<OpMovimientoInventarioEtt> Coll_OpMovimientoInventario_IdOpTraspasoDetalle { get; set; } = new List<OpMovimientoInventarioEtt>();
}